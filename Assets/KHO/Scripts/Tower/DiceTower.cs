using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceTower : Tower
{
    [SerializeField] private Sprite[] diceSprites;
    [SerializeField] private GameObject diceUIPrefab;
    [SerializeField] private Vector3 diceUIOffset = new Vector3(0, 5, 0);

    [SerializeField] private float damagePerShot;
    [SerializeField] private GameObject projectilePrefab;
    
    [SerializeField] private float attackCooldown = 2f;

    private const int DICE_ROLLS = 20;
    private const float DICEROLL_DELAY = 0.05f;
    private const float DURINGSHOT_DELAY = 0.05f;
    
    private float _launchProgress;
    private bool _isRolling = false;
    private Image _diceImageUI;

    private new void Awake()
    {
        base.Awake();
        damagePerShot = towerData.damage;
    }
    
    private new void Start()
    {
        base.Start();
        var mainUI = GameObject.FindGameObjectWithTag("Main UI");
        var diceUI = Instantiate(diceUIPrefab, mainUI.transform);
        _diceImageUI = diceUI.GetComponent<Image>();
        var rectTransform = diceUI.GetComponent<RectTransform>();
        
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + diceUIOffset;
    }

    private void Update()
    {
        if (_isRolling) return;
        
        _launchProgress += Time.deltaTime;
        if (_launchProgress >= attackCooldown)
        {
          StartCoroutine(RollTheDice());
          _launchProgress -= attackCooldown;
        }
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        _isRolling = true;
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        for (int i = 0; i <= DICE_ROLLS; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            _diceImageUI.sprite = diceSprites[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(DICEROLL_DELAY);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        if (AcquireTarget(out Transform pTarget))
        {
            if (pTarget)
            {
                TrackTarget(ref pTarget);
                StartCoroutine(ShootDelay(pTarget, finalSide + 1));
            }
        }
        else
        {
            _launchProgress = attackCooldown - 0.001f;
        }
        _isRolling = false;
    }

    private IEnumerator ShootDelay(Transform pTarget, int time)
    {
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(DURINGSHOT_DELAY);
            Shoot(pTarget);
        }
    }
    
    private void Shoot(Transform pTarget)
    {
        var projectile = PoolManager.Instance.GetProjectile(
            projectilePrefab: projectilePrefab,
            position: turret.position,
            rotation: rotatingPart.rotation * Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(-20, 20f), 0),
            target: pTarget,
            damage: damagePerShot,
            instigator: this
        );
        
        projectile.gameObject.SetActive(true);
    }
}
