using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage;

    public int reloadBulletCount;               // 총알 재장전 개수
    public int currentBulletCount;              // 현재 탄알집에 남아있는 총알 개수
    public int maxBulletCount;                  // 최대 소유 가능한 총알 개수
    public int carryBulletCount;                // 현재 소유중인 총알 개수

    public float retroActionForce;              // 반동 세기
    public float retroActionFineSightForce;     // 반동 세기

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;
}
