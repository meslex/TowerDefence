using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private GameObject bulletPrefab;

	[Header("General")]
	[SerializeField] private Transform[] muzzels;
	[SerializeField] private ParticleSystem shotVFX;
	[SerializeField] private AudioSource shotAudio;
	[SerializeField] private float fireRate = .1f;
	[SerializeField] private float damage;
	[SerializeField] private float projectileSpeed;
	[SerializeField] private float bulletLifeSpan;

	float timer;
	ObjectPooler objectPooler;

    private void Start()
    {
		objectPooler = ObjectPooler.Instance;
    }


    void Update()
	{
		timer += Time.deltaTime;

		if (Input.GetMouseButton(0) && timer >= fireRate)
		{
			for(int i = 0; i < muzzels.Length; ++i)
            {
				GameObject obj = objectPooler.GetPooledObject(bulletPrefab);
				Bullet bullet = obj.GetComponent<Bullet>();
				obj.SetActive(true);
				obj.transform.position = muzzels[i].transform.position;
				bullet.Init(projectileSpeed, damage, bulletLifeSpan, muzzels[i].transform.forward);

			}

			timer = 0f;

			if (shotVFX)
				shotVFX.Play();

			if (shotAudio)
				shotAudio.Play();
		}
	}
}
