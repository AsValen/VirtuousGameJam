using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class ExorcistEnemy : MonoBehaviour
{
    private InputAction exorcist;
    [SerializeField] private float exorcistRange = 3f; 
    private bool isExorcising = false;
    private int enemyLayer;
    private bool isWithinRange = false;

    private Collider2D[] hits;
    private GameObject currentIndicator;

    private Enemy enemyScript;
    private GameObject currentDeadEnemy;
    private Vector2 currentDeadEnemyPosition;
    private bool currentDeadEnemyStillInHits = false;
    private GameObject exorcistMask;
    private float offset = 10f; // Offset for the mask position

    private void OnEnable()
    {
        exorcist = InputSystem.actions.FindAction("Exorcist");

        if (exorcist != null)
        {
            exorcist.performed += exorcist_performed;
        }
    }

    private void OnDisable()
    {
        if (exorcist != null)
        {
            exorcist.performed -= exorcist_performed;
            exorcist = null;
        }
    }

    private void exorcist_performed(InputAction.CallbackContext context)
    {
        if(isWithinRange)
        {
            Debug.Log("Exorcising enemy: " + currentDeadEnemy.name);

            exorcistMask.SetActive(true);

            exorcistMask.transform.position = new Vector2(currentDeadEnemyPosition.x, currentDeadEnemyPosition.y + offset);

            exorcistMask.transform.DOMove(currentDeadEnemyPosition, 0.5f).SetEase(Ease.OutSine).OnComplete(() => 
            {
                //maybe need to add particle effects during tweening
                Destroy(currentDeadEnemy);
                exorcistMask.SetActive(false);
                isExorcising = false;
                resetExorcist();
            });
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isExorcising = false;
        isWithinRange = false;
        enemyLayer = LayerMask.GetMask("Enemy");
        exorcistMask = GameObject.FindGameObjectWithTag("ExorcistMask");
        exorcistMask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.OverlapCircleAll(transform.position, exorcistRange, enemyLayer);

        if (hits.Length > 0)
        {
            int i = 0;

            foreach (Collider2D hit in hits)
            {

                if(currentDeadEnemy == null && !isWithinRange)
                {
                    enemyScript = hit.gameObject.GetComponent<Enemy>();

                    if(enemyScript.IsDead)
                    {
                        currentDeadEnemy = hit.gameObject;
                        currentDeadEnemyPosition = hit.transform.position;
                        currentDeadEnemyStillInHits = true;
                        isWithinRange = true;
                    }
                }

                i++;

                if (currentDeadEnemy != null)
                {
                    Debug.Log("Current dead enemy is not null");
                    if (currentDeadEnemy == hit.gameObject)
                    {
                        currentDeadEnemyStillInHits = true;
                        break;
                        Debug.Log("Current dead enemy is still in hits");
                    } 
                    else if (currentDeadEnemy != hit.gameObject && i == hits.Length)
                    {
                        Debug.Log("Current dead enemy is not in hits anymore");
                        currentDeadEnemyStillInHits = false;
                    }
                }               
            }

            if(!currentDeadEnemyStillInHits)
            {
                resetExorcist();
            }
        }
        else
        {
            resetExorcist();
        }
    }

    private void resetExorcist()
    {
        isWithinRange = false;
        enemyScript = null;
        currentDeadEnemy = null;
        currentDeadEnemyStillInHits = false;
        currentDeadEnemyPosition = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, exorcistRange);
    }

}
