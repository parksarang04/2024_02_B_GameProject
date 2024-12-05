using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    //싱글톤 패턴
    public static BattleSystem instance { get; private set; }

    //캐릭터 배열
    public Character[] players = new Character[3];
    public Character[] enemies = new Character[3];

    //UI 요소들
    public Button attackBtn;    //공격 버튼
    public TextMeshProUGUI turnText;        //현재 턴 표시 텍스트
    public GameObject damageTextPrefab;     //데미지 표시용 프리팹
    public Canvas uiCanvas;                 //UI 캔버스

    //전투 관리 변수
    Queue<Character> turnQueue = new Queue<Character>();        //턴 순서 큐
    Character currentChar;                                      //현재 턴 캐릭터
    bool selectingTarget;                                       //타겟 선택 중인지 여부
    // Start is called before the first frame update
    private void Awake() => instance = this;

    //현재 턴 캐릭터 반환
    public Character GetCurrentChar() => currentChar;
    //공격 버튼 클릭 시 타겟 선택 모드 활성화
    void OnAttackClick() => selectingTarget = true;

    private void Start()
    {
        //캐릭터들을 속도 순으로 정렬하여 턴 순서 초기화
        var orderedChars = players.Concat(enemies).OrderByDescending(c => c.speed);

        foreach(var c in orderedChars)
        {
            turnQueue.Enqueue(c);
        }
        //공격 버튼에 이벤트 연결
        attackBtn.onClick.AddListener(OnAttackClick);
        //첫 턴 시작
        NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
        //타겟 선택 모드에서 마우스 클릭 처리
        if(selectingTarget && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Character target = hit.collider.GetComponent<Character>();
                if(target != null)
                {
                    currentChar.Attack(target);
                    ShowDamageText(target.transform.position, "20");
                    selectingTarget = false;
                    NextTurn();
                }
            }
        }
    }

    //다음 턴으로 진행
    void NextTurn()
    {
        //현재 턴 캐릭터 설정
        currentChar = turnQueue.Dequeue();
        turnQueue.Enqueue(currentChar);
        turnText.text = turnText.text = $"{currentChar.name}의 턴 (Speed:{currentChar.speed})";

        //플레이어 적 턴 처리
        if(currentChar.isPlayer)
        {
            attackBtn.gameObject.SetActive(true);   //플레이어 턴 : 공격버튼 활성화
        }
        else
        {
            attackBtn.gameObject.SetActive(false);  //적턴 : 공격버튼 비활성화
            Invoke("EnemtAttack", 1f);              // 1초후 적 공격
        }
    }

    //데이미 텍스트 생성 및 표시
    void ShowDamageText(Vector3 position, string damage)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        GameObject damageObj = Instantiate(damageTextPrefab, screenPos,Quaternion.identity, uiCanvas.transform);
        damageObj.GetComponent<TextMeshProUGUI>().text = damage;
        Destroy(damageObj,1f);
    }
    //AI의 적 공격 처리
    void EnemyAttack()
    {
        //생존한 플레이어 중 랜덤 타겟 선택
        var aliveTarget = players.Where(p=> p.gameObject.activeSelf).ToArray();

        if (aliveTarget.Length == 0) return;    //모든 플레이어 죽었으면 리턴

        var target = aliveTarget[Random.Range(0,aliveTarget.Length)];
        currentChar.Attack(target);
        ShowDamageText(target.transform.position, "20");
        NextTurn();
    }

}
