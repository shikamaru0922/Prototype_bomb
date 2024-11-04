using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;        // 爆炸特效
    public float explosionRadius = 5f;        // 爆炸半径
    public float explosionForce = 700f;       // 爆炸力
    public float upwardModifier = 1f;         // 向上修正值

    private bool hasExploded = false;

    public void Detonate()
    {
        if (hasExploded) return;

        // 显示爆炸特效
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 获取爆炸范围内的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (rb.gameObject.CompareTag("Player"))
                {
                    // 对玩家施加带有向上力的爆炸力
                    Vector3 explosionDirection = (rb.transform.position - transform.position).normalized;
                    explosionDirection.y += upwardModifier;
                    rb.AddForce(explosionDirection.normalized * explosionForce, ForceMode.Impulse);
                }
                else
                {
                    // 对其他物体施加普通的爆炸力
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        hasExploded = true;
        Destroy(gameObject); // 销毁炸弹
    }
}