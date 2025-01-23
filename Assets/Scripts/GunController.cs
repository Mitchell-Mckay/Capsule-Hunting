using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 270;
    public float gunDamage = 12.5f;

    // Variables that change throughout code
    bool _canShoot;
    [SerializeField]
    int _currentAmmoInClip;
    [SerializeField]
    int _ammoInReserve;

    //Muzzle Flash
    public Image muzzleFlashImage;
    public Sprite[] flashes;

    //audio
    [Header("Gun Sounds")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    //Aiming
    [Header("Gun Position")]
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

    public float aimSmoothing = 10;

    public Camera mainCam;

    public TextMeshProUGUI clipAmount;

    public TextMeshProUGUI reserveAmount;

    public AudioClip hitSound;

    private void Start()
    {
        mainCam = Camera.main;
        _currentAmmoInClip = clipSize;
        _ammoInReserve = reservedAmmoCapacity;
        _canShoot = true;
        muzzleFlashImage.color = new Color(0, 0, 0, 0);
        clipAmount.text = _currentAmmoInClip.ToString();
        reserveAmount.text = _ammoInReserve.ToString();
    }

    private void Update()
    {
        clipAmount.text = _currentAmmoInClip.ToString();
        reserveAmount.text = _ammoInReserve.ToString();

        DetermineAim();
        if(Input.GetMouseButton(0) && _canShoot && _currentAmmoInClip > 0) 
        {
            _canShoot = false;
            _currentAmmoInClip--;
            StartCoroutine(ShootGun());
            audioSource.PlayOneShot(RandomClip());

            // Raycast to check if the middle of the screen is pointing at an enemy
            RaycastForEnemy();
        }
        else if(Input.GetKeyDown(KeyCode.R) && _currentAmmoInClip < clipSize && _ammoInReserve > 0)
        {
            int amountNeeded = clipSize - _currentAmmoInClip;
            if(amountNeeded >= _ammoInReserve)
            {
                _currentAmmoInClip += _ammoInReserve;
                _ammoInReserve = 0;
            }
            else
            {
                _currentAmmoInClip = clipSize;
                _ammoInReserve -= amountNeeded;
            }
        }
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length-1)];
    }

    void DetermineAim()
    {
        Vector3 target = normalLocalPosition;
        if(Input.GetMouseButton(1)) target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredPosition;
    }

    void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.15f;
    }

    IEnumerator ShootGun()
    {
        DetermineRecoil();
        StartCoroutine(MuzzleFlash());

        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashImage.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleFlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.sprite = null;
        muzzleFlashImage.color = new Color(0, 0, 0, 0);
    }

    void RaycastForEnemy()
    {
        Vector3 rayOrigin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, mainCam.transform.forward, out hit))
        {
            EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(gunDamage); // Adjust the damage value as needed
                
                audioSource.PlayOneShot(hitSound, 3);
            }
        }
    }

    public void MaxAmmo()
    {
        _ammoInReserve = reservedAmmoCapacity;

        // Find and destroy the clone object
        GameObject cloneObject = GameObject.FindGameObjectWithTag("MaxAmmoClone");
        Destroy(cloneObject);
    }

    public void RootBeer()
    {
        fireRate = fireRate / 1.2f;

        // Find and destroy the clone object
        GameObject cloneObject = GameObject.FindGameObjectWithTag("RootBeerClone");
        Destroy(cloneObject);
    }
}
