using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    //�̱��� ����
    public static BattleSystem instance { get; private set; }

    //ĳ���� �迭
    public Character[] players = new Character[3];
    public Character[] enemies = new Character[3];

    //UI ��ҵ�
    public Button attackBtn;    //���� ��ư
    public TextMeshProUGUI turnText;        //���� �� ǥ�� �ؽ�Ʈ
    public GameObject damageTextPrefab;     //������ ǥ�ÿ� ������
    public Canvas uiCanvas;                 //UI ĵ����

    //���� ���� ����
    Queue<Character> turnQueue = new Queue<Character>();        //�� ���� ť
    Character currentChar;                                      //���� �� ĳ����
    bool selectingTarget;                                       //Ÿ�� ���� ������ ����
    // Start is called before the first frame update
    private void Awake() => instance = this;

    //���� �� ĳ���� ��ȯ
    public Character GetCurrentChar() => currentChar;
    //���� ��ư Ŭ�� �� Ÿ�� ���� ��� Ȱ��ȭ
    void OnAttackClick() => selectingTarget = true;

    private void Start()
    {
        //ĳ���͵��� �ӵ� ������ �����Ͽ� �� ���� �ʱ�ȭ
        var orderedChars = players.Concat(enemies).OrderByDescending(c => c.speed);

        foreach(var c in orderedChars)
        {
            turnQueue.Enqueue(c);
        }
        //���� ��ư�� �̺�Ʈ ����
        attackBtn.onClick.AddListener(OnAttackClick);
        //ù �� ����
        NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
        //Ÿ�� ���� ��忡�� ���콺 Ŭ�� ó��
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

    //���� ������ ����
    void NextTurn()
    {
        //���� �� ĳ���� ����
        currentChar = turnQueue.Dequeue();
        turnQueue.Enqueue(currentChar);
        turnText.text = turnText.text = $"{currentChar.name}�� �� (Speed:{currentChar.speed})";

        //�÷��̾� �� �� ó��
        if(currentChar.isPlayer)
        {
            attackBtn.gameObject.SetActive(true);   //�÷��̾� �� : ���ݹ�ư Ȱ��ȭ
        }
        else
        {
            attackBtn.gameObject.SetActive(false);  //���� : ���ݹ�ư ��Ȱ��ȭ
            Invoke("EnemtAttack", 1f);              // 1���� �� ����
        }
    }

    //���̹� �ؽ�Ʈ ���� �� ǥ��
    void ShowDamageText(Vector3 position, string damage)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        GameObject damageObj = Instantiate(damageTextPrefab, screenPos,Quaternion.identity, uiCanvas.transform);
        damageObj.GetComponent<TextMeshProUGUI>().text = damage;
        Destroy(damageObj,1f);
    }
    //AI�� �� ���� ó��
    void EnemyAttack()
    {
        //������ �÷��̾� �� ���� Ÿ�� ����
        var aliveTarget = players.Where(p=> p.gameObject.activeSelf).ToArray();

        if (aliveTarget.Length == 0) return;    //��� �÷��̾� �׾����� ����

        var target = aliveTarget[Random.Range(0,aliveTarget.Length)];
        currentChar.Attack(target);
        ShowDamageText(target.transform.position, "20");
        NextTurn();
    }

}
