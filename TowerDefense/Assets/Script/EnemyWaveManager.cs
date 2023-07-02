using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> fieldEnemies = new List<GameObject>(); //�t�B�[���h�ɂ���G
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform goalPos;

    //�f�[�^�֘A
    [SerializeField] private AllEnemiesDate allEnemiesDate; //�o��������G�ԍ��̎Q�Ɨp
    [SerializeField] private EnemyGenerateDate[] enemyGenerateDates; //wave���Ƃ̓G�̏o����ށA���ԁA�o���Ԋu�Q�Ɨp
    private EnemyGenerateDate currentEnemyGenerateDate; //���݂�wave�̓G�̏o����ށA���ԁA�o���Ԋu�Q�Ɨp

    //�G�o���֌W
    char[] enemySpawnOrder = null; //���݂�wave�̓G�̏o����ށA���ԕۑ��p
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
    private void ChangeState(WaveState newState)
    {
        Debug.Log(newState + "�ֈڍs");
        currentState = newState;
        stateEnter = true;
        stateTime = 0.0f;
    }
    
    private void Start()
    {
        currentState = WaveState.WaitingForStart;

        maxWave = enemyGenerateDates.Length;

        //��ԏ��߂�wave���̓ǂݍ���
        currentWave++;
        currentEnemyGenerateDate = enemyGenerateDates[0];
        enemySpawnOrder = currentEnemyGenerateDate.enemySpawnOrder;
        spawnInterval = currentEnemyGenerateDate.spawnInterval;
    }

    private void Update()
    {
        stateTime += Time.deltaTime;


        Debug.Log("���݂̃X�e�[�g" + currentState);

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
                //�Q�[���}�l�[�W���[�ɃN���A�t���O��n���\��
                break;
            
            case WaveState.GameOver:
                //�Q�[���}�l�[�W���[�ɃQ�[���I�[�o�[�t���O��n���\��
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
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {

        }

        if (Input.GetKeyUp(KeyCode.S))
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

        }

        int spawnOrderNum = currentEnemyGenerateDate.enemySpawnOrder.Length; //�X�|�[������G�̐��擾
        
        fieldEnemies.RemoveAll(enemy => enemy == null); //List����null�ɂȂ����v�f���폜����

        if (spawnOrderCount < spawnOrderNum)
        {
            //���Ԋu�œG���o��������
            if (stateTime > spawnInterval)
            {

                SpawnEnemy(allEnemiesDate.enemiesDate[(int)enemySpawnOrder[spawnOrderCount] - 48].prefab);
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

        Debug.Log("���݂�wave" + currentWave);
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
            enemySpawnOrder = currentEnemyGenerateDate.enemySpawnOrder;
            spawnInterval = currentEnemyGenerateDate.spawnInterval;

            
        }

        fieldEnemies.RemoveAll(enemy => enemy == null); //List����null�ɂȂ����v�f���폜����

        //��莞�Ԍ�ɃX�|�[���t�F�[�Y�ֈڍs
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
