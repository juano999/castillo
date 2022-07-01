using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Button shootBtn;
    [SerializeField] private Transform _spawner;
    [SerializeField] private float _projectileSpeed = 50;
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private int _projectileCount = 100;

    private float _lastFired = float.MinValue;
    private bool _fired;

    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        //shootBtn.onClick.AddListener(ShootHandler);
        shootBtn.onClick.AddListener(ShootHandler);
    }

    //private void Update()
    //{
    //    if (!IsOwner) return;
    //}

    private void ShootHandler()
    {
        if (!IsOwner) return;

        //if (_lastFired + _cooldown < Time.time && _projectileCount > 0)
        if (_projectileCount > 0)
        {
            _lastFired = Time.time;
            Vector3 dir;           
            if (transform.localScale.x > 0.0f) dir = Vector3.right;
            else dir = Vector3.left;
            Debug.Log("dir: " + dir);

            // Send off the request to be executed on all clients
            RequestFireServerRpc(dir);

            // Fire locally immediately
            ExecuteShoot(dir);
            --_projectileCount;
        }
        if(_projectileCount == 0)
        {
            Debug.Log("recargando balas.. Espera 5 segundos");
            Invoke(nameof(ReloadProjectiles), 5);
        }
    }

    public void ReloadProjectiles()
    {
        _projectileCount = 100;
    }

    [ServerRpc]
    private void RequestFireServerRpc(Vector3 dir)
    {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir)
    {
        if (!IsOwner) ExecuteShoot(dir);
    }

    private void ExecuteShoot(Vector3 dir)
    {
        var projectile = Instantiate(_projectile, _spawner.position, Quaternion.identity);
        projectile.Init(dir * _projectileSpeed);

    }


    //private void Shoot()
    //{
    //    Vector3 direction;
    //    if (transform.localScale.x > 0.0f) direction = Vector2.right;
    //    else direction = Vector3.left;

    //    GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
    //    bullet.GetComponent<BulletScript>().SetDirection(direction);

    //}
}
