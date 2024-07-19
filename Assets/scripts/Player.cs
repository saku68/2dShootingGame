using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 4;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    // [SerializeField]
     // private bool _isSpeedBoostActive = false;
    private bool _isTripleShotActive = false;
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualiser;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootLaser();
        }
        
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // transform.Translate(new Vector3(horizontalInput, verticalInput, 0) *_speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        // if (_isSpeedBoostActive == false)
        // {
        transform.Translate(direction * _speed * Time.deltaTime);
        // }
        // else
        // {
            // transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        // }

        if (transform.position.y >= 3)
        {
            transform.position = new Vector3(transform.position.x, 3, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        }
        void ShootLaser()
        {

            if (_isTripleShotActive == true)
            {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
        
            _audioSource.Play();

        }

        public void Damage()
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shieldVisualiser.SetActive(false);
                return;
            }

            _lives -= 1;

            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                        
                Destroy(this.gameObject);

                 _spawnManager.OnPlayerDeath();     
                 
            }
        }

        public void TripleShotActive()
        {
            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

        IEnumerator TripleShotPowerDownRoutine()
        {
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
        }
        
        public void SpeedBoostActive()
        {
            // _isSpeedBoostActive = true;
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }

        IEnumerator SpeedBoostPowerDownRoutine()
        {
            yield return new WaitForSeconds(10.0f);
            // _isSpeedBoostActive = false;
            _speed /= _speedMultiplier;
        }

        public void ShieldsActive()
        {
            _isShieldsActive = true;
            _shieldVisualiser.SetActive(true);
        }

        public void AddScore(int points)
        {
            _score += points;
            _uiManager.UpdateScore(_score);
        }
}

