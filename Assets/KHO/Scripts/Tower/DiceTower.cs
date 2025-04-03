using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceTower : Tower
{
    private const int DICE_ROLLS = 20;
    private const float DICEROLL_DELAY = 0.05f;
    private const float DURINGSHOT_DELAY = 0.05f;
    
    [SerializeField] private Sprite[] diceSprites;
    [SerializeField] private GameObject diceUIPrefab;
    [SerializeField] private Vector3 diceUIOffset = new(0, 5, 0);

    [SerializeField] private float damagePerShot;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float attackCooldown = 2f;
    
    private Image _diceImageUI;
    private bool _isRolling;
    private float _launchProgress;
    private GameObject _diceUI;
    
    private new void Start()
    {
        base.Start();
        var mainUI = GameObject.FindGameObjectWithTag("Main UI");

        _diceUI = Instantiate(diceUIPrefab, mainUI.transform);
        _diceImageUI = _diceUI.GetComponent<Image>();
        var rectTransform = _diceUI.GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + diceUIOffset;

        OnRarityChanged();
    }

    private new void Update()
    {
        base.Update();
        
        if (_diceImageUI)
        {
            if (Game.Instance.IsPlayingSlowmo)
            {
                _diceImageUI.enabled = false;
            }
            else if (!Game.Instance.IsPlayingSlowmo)
            {
                _diceImageUI.enabled = true;
                _diceImageUI.transform.position =
                    Camera.main.WorldToScreenPoint(turret.transform.position) + diceUIOffset;
            }
        }

        if (_isRolling) return;

        _launchProgress += Time.deltaTime;
        if (_launchProgress >= attackCooldown)
        {
            StartCoroutine(RollTheDice());
            _launchProgress -= attackCooldown;
        }
    }

    protected override void OnRarityChanged()
    {
        damagePerShot = towerData.TowerStats[(int)Rarity].damage;
        attackCooldown = towerData.TowerStats[(int)Rarity].attackSpeed;
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        _isRolling = true;
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        var randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        var finalSide = 0;

        // Loop to switch dice sides ramdomly
        for (var i = 0; i <= DICE_ROLLS; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            _diceImageUI.sprite = diceSprites[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(DICEROLL_DELAY);
        }

        finalSide = randomDiceSide + 1;

        if (AcquireTarget(out var pTarget))
        {
            if (pTarget)
            {
                TrackTarget(ref pTarget);
                StartCoroutine(ShootDelay(pTarget, finalSide));
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
        for (var i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(DURINGSHOT_DELAY);
            Shoot(pTarget);
        }
    }

    private void Shoot(Transform pTarget)
    {
        var damagePacket = new DamagePacket(damagePerShot, towerData.elementType, this);

        var projectile = PoolManager.Instance.GetProjectile(
            projectilePrefab,
            turret.position,
            rotatingPart.rotation * Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(-20, 20f), 0),
            pTarget,
            damagePacket
        );

        projectile.gameObject.SetActive(true);
        
        AudioManager.instance.PlaySound(SoundEffect.DiceRoll);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        
        Destroy(_diceUI);
    }
}