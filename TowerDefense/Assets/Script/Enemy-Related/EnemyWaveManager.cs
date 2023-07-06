using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private UISystemManager uISytemManager;
    public int defencePoint = 10; //�˔j���Ă��悢�G�̌��E��
    private List<GameObject> fieldEnemies = new List<GameObject>(); //�t�B�[���h�ɂ���G
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform goalPos;

    //�f�[�^�֘A
    [SerializeField] private AllEnemiesDate allEnemiesDate; //�o��������G�ԍ��̎Q�Ɨp
    [SerializeField] private EnemyGenerateDate[] enemyGenerateDates; //wave���Ƃ̓G�̏o����ށA���ԁA�o���Ԋu�Q�Ɨp
    private EnemyGenerateDate currentEnemyGenerateDate; //���݂�wave�̓G�̏o����ށA���ԁA�o���Ԋu�Q�Ɨp

    //�G�o���֌W
    EnemySpawnOrder[] enemySpawnOrders = null; //���݂�wave�̓G�̏o����ށA���ԕۑ��p
    int spawnOrderCount = 0; //���݁A���Ԗڂ̓G���o���Ă��邩�̕ۑ��p

    //Wave�֌W
    private int currentWave = 0; //���݂�wave��
    private int maxWave; //wave�̐�
    private float spawnInterval; //���݂�wave�̓G�̏o���Ԋu�Q�Ɨp
    [SerializeField] private float waveDuration = 10.0f; //Wave�̎�������,�@���ׂĂ̓G���o�����Ă���(=waveState�ڍs��)�J�E���g�J�n
    [SerializeField] private float preprationTime; //��������(=preparationState�̎���)

    //State�̎��
    public enum WaveState
    {
        WaitingForStart,
        SpawnWave,
        Wave,
        Preparation,
        GameClear,
        GameOver
    }
    private WaveState currentState; //���݂̃X�e�[�g
    private bool stateEnter = false; //�X�e�[�g�ύX���Ă���T��ڂ̃t���[���ł��邱�Ƃ�\��
    private float stateTime = 0.0f; //�X�e�[�g�Ɉڍs���Ă���̎��Ԃ�ۑ�

    /// <summary>
    /// �X�e�[�g�ڍs�֐�
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(WaveState newState)
    {
        currentState = newState;
        stateEnter = true;
        stateTime = 0.0f;
        Debug.Log("���݂̃X�e�[�g" + currentState);
    }
    
    private void Start()
    {
        currentState = WaveState.WaitingForStart;
        Debug.Log("���݂̃X�e�[�g" + currentState);

        maxWave = enemyGenerateDates.Length;

        //��ԏ��߂�wave���̓ǂݍ���
        currentWave++;
        currentEnemyGenerateDate = enemyGenerateDates[0];
        enemySpawnOrders = currentEnemyGenerateDate.enemySpawnOrder;
        spawnInterval = currentEnemyGenerateDate.spawnInterval;
       
        //UI���̓���
        uISytemManager.InputEnemyTotal(currentEnemyGenerateDate.enemySpawnOrder.Length);
        uISytemManager.InputDefencePoint(defencePoint);
    }

    private void Update()
    {
        stateTime += Time.deltaTime;

        if (defencePoint <= 0 && currentState != WaveState.GameOver)
        {
            ChangeState(WaveState.GameOver);
        }

        // �X�e�[�g���Ƃ̐U�镑������
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
                GameManager.instance.GameCrear(); //�Q�[���N���A�̃t���O�𗧂Ă�
                break;
            
            case WaveState.GameOver:
                GameManager.instance.GameOver(); //�Q�[���I�[�o�[�̃t���O�𗧂Ă�
                break;

        }
    }

    //�X�e�[�g�ڍs��A�P�t���[���ڂ̂ݓ��������߂̏���
    private void LateUpdate()
    {
        if (stateTime != 0.0f && stateEnter)
        {
            stateEnter = false;
        }
    }


    /// <summary>
    /// �X�^�[�g����܂ł̑ҋ@�A��������
    /// </summary>
    private void UpdateWaitngForStartState()
    {
        if (Input.GetKeyUp(KeyCode.S) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ChangeState(WaveState.SpawnWave);
        }
    }

    /// <summary>
    /// �X�|�[���t�F�[�Y�A���ׂĂ̓G�̏o����uwaveState�v�Ɉڍs
    /// </summary>
    private void UpdateSpawnWaveState()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {
            spawnOrderCount = 0;
        }

        int spawnOrderNum = currentEnemyGenerateDate.enemySpawnOrder.Length; //�X�|�[������G�̐��擾
        
        fieldEnemies.RemoveAll(enemy => enemy == null); //List����null�ɂȂ����v�f���폜����

        if (spawnOrderCount < spawnOrderNum)
        {
            //���Ԋu�œG���o��������
            if (stateTime > spawnInterval)
            {
                //�o��������G�v���n�u�A���x��
                EnemySpawnOrder enemySpawnOrder = enemySpawnOrders[spawnOrderCount];
                GameObject spawnEnemy = allEnemiesDate.enemiesDate[enemySpawnOrder.EnemyID].prefab;
                int level = enemySpawnOrder.level;
                
                SpawnEnemy(spawnEnemy, level);

                spawnOrderCount++;
                stateTime = 0.0f;
            }
        }
        else
        {
            //�G�����ׂďo����������AWaveState�Ɉڍs
            ChangeState(WaveState.Wave);
        }
    }

    /// <summary>
    /// �퓬�t�F�[�Y�A�u�G�̑S�Łvor�u��莞�Ԍo�ߌ�v�ɃX�e�[�g�ڍs(�ڍs��́A�����t�F�[�Yor�N���A�t�F�[�Y)
    /// </summary>
    private void UpdateWaveState()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {

        }
        
        fieldEnemies.RemoveAll(enemy => enemy == null); //List����null�ɂȂ����v�f���폜����

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
    /// �����t�F�[�Y�A��莞�Ԍ�ɃX�|�[���t�F�[�Y�Ɉȍ~
    /// </summary>
    private void UpdatePreparationState()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {
            //����Wave���̎擾
            currentWave++;
            currentEnemyGenerateDate = enemyGenerateDates[currentWave - 1];
            enemySpawnOrders = currentEnemyGenerateDate.enemySpawnOrder;
            spawnInterval = currentEnemyGenerateDate.spawnInterval;
            uISytemManager.InputEnemyTotal(currentEnemyGenerateDate.enemySpawnOrder.Length);

        }

        fieldEnemies.RemoveAll(enemy => enemy == null); //List����null�ɂȂ����v�f���폜����

        //��莞�Ԍ�ɃX�|�[���t�F�[�Y�ֈڍs
        if (stateTime > preprationTime)
        {
            ChangeState(WaveState.SpawnWave);
        }
    }

    private void SpawnEnemy(GameObject _spawnEnemy, int _level)
    {
        if (_spawnEnemy == null)
        {
            return;
        }

        GameObject enemy = Instantiate(_spawnEnemy, spawnPos.position, Quaternion.identity);
        enemy.GetComponent<EnemyScript>().InputEnemyInformation(goalPos, _level);
        fieldEnemies.Add(enemy);
    }


    /// <summary>
    /// �G���X�e�[�W��˔j�����ۂ̏���
    /// </summary>
    public void PassedEnemy()
    {
        defencePoint = defencePoint < 1 ? 0 :  defencePoint - 1;
        uISytemManager.InputDefencePoint(defencePoint);
        if(defencePoint < 1)
        {
            ChangeState(WaveState.GameOver);
        }
    }
}
