using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : Singleton<FireManager>
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] List<GameObject> _bulletPool;

    GameObject BulletContainer;

    private void Start()
    {
        BulletContainer = new GameObject();
        BulletContainer.transform.SetParent(transform);
        _bulletPool = new List<GameObject>();
        GameManager.Instance.OnGameLevelChanged.AddListener(HandleGameLevelChanged);
    }

    void HandleGameLevelChanged(string levelName)
    {
        foreach(GameObject bullet in _bulletPool)
        {
            bullet.SetActive(false);
        }
    }
    public void Fire(Bullet.BulletInfo bulletInfo)
    {
        GetBullet(bulletInfo).GetComponent<Bullet>().Fire();
    }

    private GameObject GetBullet(Bullet.BulletInfo bulletInfo)
    {
        foreach(GameObject go in _bulletPool)
        {
            if (!go.activeInHierarchy)
            {
                go.GetComponent<Bullet>().UpdateValues(bulletInfo);
                go.SetActive(true);
                return go;
            }
        }
        GameObject bullet = Instantiate(_bulletPrefab, BulletContainer.transform);
        bullet.GetComponent<Bullet>().UpdateValues(bulletInfo);
        _bulletPool.Add(bullet);
        return bullet;
    }
    private void OnDisable()
    {
        foreach (GameObject go in _bulletPool)
        {
            go.SetActive(false);
        }
    }
}
