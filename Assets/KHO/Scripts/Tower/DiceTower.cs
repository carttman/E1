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

    private Image diceImageUI;

    private void Start()
    {
        var mainUI = GameObject.FindGameObjectWithTag("Main UI");
        var diceUI = Instantiate(diceUIPrefab, mainUI.transform);
        diceImageUI = diceUI.GetComponent<Image>();
        var rectTransform = diceUI.GetComponent<RectTransform>();
        
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + diceUIOffset;
    }

    private void OnMouseDown()
    {
        if (diceImageUI == null) return;
        StartCoroutine(nameof(RollTheDice));
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
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
    }

    private void Shoot(Transform pTarget)
    {
        var newProjectile = Instantiate(projectilePrefab, turret.position, Quaternion.identity);
        newProjectile.transform.LookAt(pTarget);
        newProjectile.transform.Rotate(Vector3.up, Random.Range(50f, 75f));
        
        var proj = newProjectile.GetComponent<Projectile>();
        proj.Target = pTarget;
        proj.Damage = damagePerShot;
        proj.instigator = this;
    }
}
