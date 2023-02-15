using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sword;
    SoundManager sound;

    [Header("Increase Attribute")]
    [SerializeField] float addHealth;
    [SerializeField] float addAttack;
    [SerializeField] int addBullet;

    private void Awake()
    {
        sound = FindObjectOfType<SoundManager>();
    }

    private void OnEnable()
    {
        Destroy(gameObject, 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sound.PlaySound("Collectible");
            Player player = other.gameObject.GetComponent<Player>();
            Weapon weapon = other.gameObject.GetComponent<Weapon>();

            if (this.gameObject == health)
            {
                AddHealth(player);
                Destroy(gameObject);
            }

            else if (this.gameObject == bullet)
            {
                player.bulletLeft += addBullet;
                Destroy(gameObject);
            }

            else if (this.gameObject == sword)
            {
                weapon.weapon1Damage += addAttack;
                weapon.weapon2Damage += addAttack / 2;
                Destroy(gameObject);
            }
        }
    }

    private void AddHealth(Player player)
    {
        if (player.playerCurrentHealth <= player.playerMaxHealth)
        {
            player.playerCurrentHealth += addHealth;
            if (player.playerCurrentHealth > player.playerMaxHealth)
            {
                player.playerCurrentHealth = player.playerMaxHealth;
            }
        }
    }
}
