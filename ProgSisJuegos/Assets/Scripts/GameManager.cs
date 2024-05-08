using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Manager settings")]
    public string deathScene = "Death";
    public string winScene = "Win";

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

    private List<string> _unlockableIds= new List<string>();

    private EventQueue _eventQueue;
    public EventQueue EventsCommandsQueue => _eventQueue;

    private void Awake()
    {
        _eventQueue = GetComponent<EventQueue>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _thePlayer.OnPlayerDeath += OnPlayerDeath;
        InitializeFactoryMonsters();
    }

    private void InitializeFactoryMonsters()
    {
        _monstersFactory = new FactoryMonsters(_monsters);
    }

    public void PlayUISound(AudioClip clip, float volumeScale = 1)
    {
        _audioSource.PlayOneShot(clip, volumeScale);
    }

    public void OnPillsGrab(ItemPills pillRef)
    {
        _thePlayer?.AnyDamage(-pillRef.ItemData.HealAmount);
    }

    public void GiveWeaponToPlayer(WeaponTypes type)
    {
        _thePlayer.OnGiveWeapon?.Invoke(type);
    }

    // Locked door

    public void OnPlayerKeyGrab(string id)
    {
        _unlockableIds.Add(id);
    }

    public bool OnPlayerHasKey(string id)
    {
        return _unlockableIds.Contains(id);
    }

    public EnemyBase CreateMonster(string monsterID)
    {
        return _monstersFactory.CreateProduct(monsterID);
    }

    // Switching scenes

    private void OnPlayerDeath()
    {
        _audioSource.PlayOneShot(_deathClips[Random.Range(0, _deathClips.Count - 1)]);
        _uiManager.OnDeathFade?.Invoke();

        _thePlayer.OnPlayerDeath -= OnPlayerDeath;
        _thePlayer.gameObject.SetActive(false);
        deathCam.SetActive(true);

        StartCoroutine(DeathSwitchLevel());
    }

    public void OnGameFinised()
    {
        _uiManager.OnCameraFade(false, 5, Color.black);
        StartCoroutine(GameOverSwitchScene());
    }

    IEnumerator GameOverSwitchScene()
    {
        yield return new WaitForSeconds(6);
        StartCoroutine(GotoLevel(winScene));
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
