using UnityEngine;

public class lastPosition : MonoBehaviour
{
    private Vector2 last_Position;

    void Update()
    {
        // ������Ʈ�� ��ġ�� ������Ʈ
       last_Position = transform.position;
        
    }

    // ������ ��ġ�� ��ȯ�ϴ� �Լ�
    public Vector2 GetLastPosition()
    {
        return last_Position;
    }
}