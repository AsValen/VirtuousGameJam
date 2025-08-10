using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public class ExorcistEnemy : MonoBehaviour
{
    private InputAction exorcist;
    [SerializeField] private float exorcistRange = 3f; 
    private bool isExorcising = false;
    private int enemyLayer;
    private bool isWithinRange = false;

    private Collider2D[] hits;
    private GameObject currentIndicator;

    //private Enemy enemyHealth;
    private EnemyHealth enemyHealth;
    private GameObject currentDeadEnemy;
    private Vector2 currentDeadEnemyPosition;
    private bool currentDeadEnemyStillInHits = false;
    private GameObject exorcistMask;
    private float offset = 2f; // Offset for the mask position

    [SerializeField] private int healingAmount = 1;

    private AudioSource audioSource;
    [SerializeField] private AudioClip exorcistSound;
    [SerializeField] private AudioClip enemyExorcistedSound;

    public static event Action<int> OnExorcistEnemy;


    public bool IsExorcising
    {
        get => isExorcising;
    }

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
            playSound(exorcistSound);

            Debug.Log("Exorcising enemy: " + currentDeadEnemy.name);

            exorcistMask.SetActive(true);

            currentDeadEnemyPosition = currentDeadEnemy.transform.position;
            exorcistMask.transform.position = new Vector2(currentDeadEnemyPosition.x, currentDeadEnemyPosition.y + offset);

            exorcistMask.transform.DOMove(currentDeadEnemyPosition, 0.5f).SetEase(Ease.OutSine).OnComplete(() => 
            {
                playSound(enemyExorcistedSound);
                //maybe need to add particle effects during tweening
                Destroy(currentDeadEnemy);
                exorcistMask.SetActive(false);
                isExorcising = false;
                OnExorcistEnemy?.Invoke(healingAmount);
                resetExorcist();
            });
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isExorcising = false;
        isWithinRange = false;
        enemyLayer = LayerMask.NameToLayer("Enemy");
        //exorcistMask = GameObject.FindGameObjectWithTag("ExorcistMask");
        //exorcistMask.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.OverlapCircleAll(transform.position, exorcistRange);

        if (hits.Length > 0)
        {
            int i = 0;

            foreach (Collider2D hit in hits)
            {
                Debug.Log(hit.gameObject.layer);
                if(hit.gameObject.layer == enemyLayer)
                {
                    if(currentDeadEnemy == null && !isWithinRange)
                    {
                        enemyHealth = hit.gameObject.GetComponentInChildren<EnemyHealth>();

                        Debug.Log("within Enemy");
                        Debug.Log(enemyHealth.IsDead);

                        if (enemyHealth.IsDead)
                        {
                            Debug.Log("enemy dead");
                            Debug.Log(hit.gameObject);
                            exorcistMask = FindChildWithTag(hit.gameObject, "ExorcistMask");
                            exorcistMask.SetActive(false);
                            currentDeadEnemy = hit.gameObject;
                            currentDeadEnemyStillInHits = true;
                            isWithinRange = true;
                        }
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
        exorcistMask = null;
        isWithinRange = false;
        enemyHealth = null;
        currentDeadEnemy = null;
        currentDeadEnemyStillInHits = false;
        currentDeadEnemyPosition = Vector2.zero;
    }

    private void playSound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, exorcistRange);
    }

}
