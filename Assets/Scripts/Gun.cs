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

    public int reloadBulletCount;               // �Ѿ� ������ ����
    public int currentBulletCount;              // ���� ź������ �����ִ� �Ѿ� ����
    public int maxBulletCount;                  // �ִ� ���� ������ �Ѿ� ����
    public int carryBulletCount;                // ���� �������� �Ѿ� ����

    public float retroActionForce;              // �ݵ� ����
    public float retroActionFineSightForce;     // �ݵ� ����

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;
}
