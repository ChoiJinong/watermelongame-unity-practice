//using UnityEngine;

//public class Planet : MonoBehaviour
//{
//    public GameManager gameManager;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        // 같은 행성 충돌
//        if (this.gameObject.tag == collision.gameObject.tag)
//        {
//            // 같은 오브젝트라도 식별할 수 있도록 고유 아이디값을 비교
//            if (this.gameObject.GetInstanceID() > collision.gameObject.GetInstanceID())
//            {
//                GameObject clone = Instantiate(gameManager.planets[int.Parse(this.gameObject.tag)]);
//                clone.transform.position = transform.position;
//                clone.GetComponent<CircleCollider2D>().enabled = true;
//                clone.GetComponent<Rigidbody2D>().gravityScale = 1;
//                clone.GetComponent<Planet>().gameManager = gameManager; // Planet이 가지고 있던 매니저를 clone에게 전달
//            }
//            Destroy(this.gameObject);
//        }
//    }
//}


using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameManager gameManager; // GameManager 참조 (다음 행성 생성할 때 필요)
    private bool isMerging = false; // 현재 이 행성이 합성 중인지 여부 (중복 합성 방지)
    float t = 0.0f;

    void Update()
    {
        if (transform.position.y > 4.1f)
        {
            t += Time.deltaTime;
            if (t > 3.0f)
                gameManager.GameOver();
        }
        else
            t = 0.0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 상대 오브젝트에서 Planet 스크립트를 가져옴
        Planet other = collision.gameObject.GetComponent<Planet>();

        // 상대가 Planet이 아니면 (벽, 바닥 등) 아무 처리 안 함
        if (other == null) return;
        // 이미 나 또는 상대가 합성 중이면 중복 처리 방지
        if (isMerging || other.isMerging) return;

        // 같은 종류(태그)의 행성끼리만 합성
        if (gameObject.tag == collision.gameObject.tag)
        {
            // 두 오브젝트 중 하나만 합성 처리하도록 ID로 구분
            if (gameObject.GetInstanceID() > collision.gameObject.GetInstanceID())
            {
                // 합성 시작 → 둘 다 잠금
                isMerging = true;
                other.isMerging = true;

                // 추가 충돌 방지 (물리적으로도 막음)
                CircleCollider2D myCol = GetComponent<CircleCollider2D>();
                CircleCollider2D otherCol = other.GetComponent<CircleCollider2D>();

                if (myCol != null) myCol.enabled = false;
                if (otherCol != null) otherCol.enabled = false;

                // 다음 단계 행성 인덱스 (현재 tag를 숫자로 사용 중)
                int nextIndex = int.Parse(gameObject.tag);
                int currentLevel = int.Parse(gameObject.tag);
                int nextLevel = currentLevel + 1;

                // 새로운 상위 단계 행성 생성 (합쳐진 위치에)
                GameObject newPlanet = Instantiate(
                    gameManager.planets[nextIndex],
                    transform.position,
                    Quaternion.identity
                );
                gameManager.AddScore(nextLevel);

                // 새 행성 설정
                newPlanet.GetComponent<Rigidbody2D>().gravityScale = 1; // 떨어지게
                newPlanet.GetComponent<CircleCollider2D>().enabled = true; // 충돌 가능
                newPlanet.GetComponent<Planet>().gameManager = gameManager; // 매니저 연결

                // 기존 두 행성 제거 (합성 완료)
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}