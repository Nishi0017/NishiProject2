using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> fieldEnemies = new List<GameObject>(); //フィールドにいる敵
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform goalPos;

    //データ関連
    [SerializeField] private AllEnemiesDate allEnemiesDate; //出現させる敵番号の参照用
    [SerializeField] private EnemyGenerateDate[] enemyGenerateDates; //waveごとの敵の出現種類、順番、出現間隔参照用
    private EnemyGenerateDate currentEnemyGenerateDate; //現在のwaveの敵の出現種類、順番、出現間隔参照用

    //敵出現関係
    char[] enemySpawnOrder = null; //現在のwaveの敵の出現種類、順番保存用
    int spawnOrderCount = 0; //現在、何番目の敵を出しているかの保存用

    //Wave関係
    private int currentWave = 0; //現在のwave数
    private int maxWave; //waveの数
    private float spawnInterval; //現在のwaveの敵の出現間隔参照用
    [SerializeField] private float waveDuration = 10.0f; //Waveの持続時間,　すべての敵が出現してから(=waveState移行後)カウント開始
    [SerializeField] private float preprationTime; //準備時間(=preparationStateの時間)

    //Stateの種類
    public enum WaveState
    {
        WaitingForStart,
        SpawnWave,
        Wave,
        Preparation,
        GameClear,
        GameOver
    }
    private WaveState currentState; //現在のステート
    private bool stateEnter = false; //ステート変更してからⅠ回目のフレームであることを表す
    private float stateTime = 0.0f; //ステートに移行してからの時間を保存

    /// <summary>
    /// ステート移行関数
    /// </summary>
    /// <param name="newState"></param>
    private void ChangeState(WaveState newState)
    {
        Debug.Log(newState + "へ移行");
        currentState = newState;
        stateEnter = true;
        stateTime = 0.0f;
    }
    
    private void Start()
    {
        currentState = WaveState.WaitingForStart;

        maxWave = enemyGenerateDates.Length;

        //一番初めのwave情報の読み込み
        currentWave++;
        currentEnemyGenerateDate = enemyGenerateDates[0];
        enemySpawnOrder = currentEnemyGenerateDate.enemySpawnOrder;
        spawnInterval = currentEnemyGenerateDate.spawnInterval;
    }

    private void Update()
    {
        stateTime += Time.deltaTime;


        Debug.Log("現在のステート" + currentState);

        // ステートごとの振る舞い実装
        switch (currentState)
        {
            case WaveState.WaitingForStart:
                UpdateWaitngForStartState();
                break;

            case WaveState.SpawnWave:
                UpdateSpawnWaveState();
                break;

            case WaveState.Wave:
                UpdateWaveState();
                break;

            case WaveState.Preparation:
                UpdatePreparationState();
                break;

            case WaveState.GameClear:
                //ゲームマネージャーにクリアフラグを渡す予定
                break;
            
            case WaveState.GameOver:
                //ゲームマネージャーにゲームオーバーフラグを渡す予定
                break;

        }
    }

    //ステート移行後、１フレーム目のみ動かすための処理
    private void LateUpdate()
    {
        if (stateTime != 0.0f && stateEnter)
        {
            stateEnter = false;
        }
    }


    /// <summary>
    /// スタートするまでの待機、準備時間
    /// </summary>
    private void UpdateWaitngForStartState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ChangeState(WaveState.SpawnWave);
        }
    }

    /// <summary>
    /// スポーンフェーズ、すべての敵の出現後「waveState」に移行
    /// </summary>
    private void UpdateSpawnWaveState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }

        int spawnOrderNum = currentEnemyGenerateDate.enemySpawnOrder.Length; //スポーンする敵の数取得
        
        fieldEnemies.RemoveAll(enemy => enemy == null); //List内でnullになった要素を削除する

        if (spawnOrderCount < spawnOrderNum)
        {
            //一定間隔で敵を出現させる
            if (stateTime > spawnInterval)
            {

                SpawnEnemy(allEnemiesDate.enemiesDate[(int)enemySpawnOrder[spawnOrderCount] - 48].prefab);
                spawnOrderCount++;
                stateTime = 0.0f;
            }
        }
        else
        {
            //敵をすべて出現させたら、WaveStateに移行
            ChangeState(WaveState.Wave);
        }
    }

    /// <summary>
    /// 戦闘フェーズ、「敵の全滅」or「一定時間経過後」にステート移行(移行先は、準備フェーズorクリアフェーズ)
    /// </summary>
    private void UpdateWaveState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }
        
        fieldEnemies.RemoveAll(enemy => enemy == null); //List内でnullになった要素を削除する

        Debug.Log("現在のwave" + currentWave);
        if(currentWave != maxWave)
        {
            if(fieldEnemies.Count == 0 || stateTime > waveDuration)
            {
                ChangeState(WaveState.Preparation);
            }
        }
        else
        {
            if(fieldEnemies.Count == 0)
            {
                ChangeState(WaveState.GameClear);
            }
        }
    }

    /// <summary>
    /// 準備フェーズ、一定時間後にスポーンフェーズに以降
    /// </summary>
    private void UpdatePreparationState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {
            //次のWave情報の取得
            currentWave++;
            currentEnemyGenerateDate = enemyGenerateDates[currentWave - 1];
            enemySpawnOrder = currentEnemyGenerateDate.enemySpawnOrder;
            spawnInterval = currentEnemyGenerateDate.spawnInterval;

            
        }

        fieldEnemies.RemoveAll(enemy => enemy == null); //List内でnullになった要素を削除する

        //一定時間後にスポーンフェーズへ移行
        if (stateTime > preprationTime)
        {
            ChangeState(WaveState.SpawnWave);
        }
    }

    private void SpawnEnemy(GameObject spawnEnemy)
    {
        if (spawnEnemy == null)
        {
            return;
        }

        GameObject enemy = Instantiate(spawnEnemy, spawnPos.position, Quaternion.identity);
        enemy.GetComponent<Test_HumanScript>().goalPosition = goalPos;
        fieldEnemies.Add(enemy);
    }
}
