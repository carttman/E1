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
    private float launchProgress;
    
    private bool _isRolling = false;

    private Image diceImageUI;

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
        diceImageUI = diceUI.GetComponent<Image>();
        var rectTransform = diceUI.GetComponent<RectTransform>();
        
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + diceUIOffset;
    }

    private void Update()
    {
        if (_isRolling) return;
        
        launchProgress += Time.deltaTime;
        if (launchProgress >= attackCooldown)
        {
          StartCoroutine(RollTheDice());
          launchProgress -= attackCooldown;
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
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            diceImageUI.sprite = diceSprites[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        if (AcquireTarget(out Transform pTarget))
        {
            if (pTarget)
            {
                for (int i = 0; i < finalSide; i++)
                {
                    Shoot(pTarget);
                }
            }
        }
        else
        {
            launchProgress = attackCooldown - 0.001f;
        }
        _isRolling = false;
    }
    
    private void Shoot(Transform pTarget)
    {
        var newProjectile = Instantiate(projectilePrefab, turret.position, Quaternion.identity);
        newProjectile.transform.LookAt(pTarget);
        newProjectile.transform.Rotate(Vector3.up, Random.Range(-20f, 20f));
        newProjectile.transform.Rotate(Vector3.right, Random.Range(-50f, 50f));
        
        var proj = newProjectile.GetComponent<Projectile>();
        proj.Target = pTarget;
        proj.Damage = damagePerShot;
        proj.instigator = this;
    }
}
