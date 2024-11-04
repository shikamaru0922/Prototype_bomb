using UnityEngine;

public class RingScore : MonoBehaviour
{
    public int scoreValue = 1;   // 通过环获得的分数

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 增加分数
            other.GetComponent<PlayerController>().playerScore += scoreValue;

            Debug.Log(other.GetComponent<PlayerController>().playerScore);
            // 可以在此处添加其他效果，例如播放声音、特效等

            // 销毁环，或者禁用它，防止重复得分
            Destroy(gameObject);
        }
    }
}