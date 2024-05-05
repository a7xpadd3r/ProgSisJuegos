using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Manager settings")]
    public string deathScene = "Death";

    [Header("Manager references")]
    [SerializeField] private PlayerController _thePlayer;
    [SerializeField] private UIManager _uiManager;    
    [SerializeField] private GameObject deathCam;

    [Header("Audio stuff")]
    [SerializeField] private List<AudioClip> _deathClips;
    private static AudioSource _audioSource;

    [Header("Factory references")]
    [SerializeField] private List<MonsterDatabase> _monsters;
    private FactoryMonsters _monstersFactory;
    public Transform testingow;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _thePlayer.OnPlayerDeath += OnPlayerDeath;
        
        _monstersFactory = new FactoryMonsters(_monsters);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            EnemyBase enemyToSpawn = _monstersFactory.CreateProduct(nameof(MonsterType.Wheelchair));
            Debug.Log($"Trying to spawn '{enemyToSpawn}'...");
            Instantiate(enemyToSpawn, testingow.transform.position, Quaternion.identity);
        }
    }

    public void PlayUISound(AudioClip clip, float volumeScale = 1)
    {
        _audioSource.PlayOneShot(clip, volumeScale);
    }

    public void GiveWeaponToPlayer(WeaponTypes type)
    {
        _thePlayer.OnGiveWeapon?.Invoke(type);
    }

    private void OnPlayerDeath()
    {
        _audioSource.PlayOneShot(_deathClips[Random.Range(0, _deathClips.Count - 1)]);
        _uiManager.DeathFade();

        _thePlayer.OnPlayerDeath -= OnPlayerDeath;
        _thePlayer.gameObject.SetActive(false);
        deathCam.SetActive(true);

        StartCoroutine(DeathSwitchLevel());
    }

    IEnumerator DeathSwitchLevel()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(GotoLevel(deathScene));
    }

    IEnumerator GotoLevel(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
            yield return null;
    }
}
