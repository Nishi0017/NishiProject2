using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //�Q�[�������ǂ����̔���
    private bool isPlaying = true;
    //�Q�[���N���A����
    private bool isGameCleared = false;
    //�Q�[���I�[�o�[����
    private bool isGameOvered = false;

    //�v���p�e�B�o�R�ŕϐ��̒l���擾����
    public static GameManager Instance { get { return instance; } }
    public bool IsPlaying { get { return isPlaying; } }
    public bool IsGameCleared { get { return isGameCleared; } }
    public bool IsGameOvered { get {  return isGameOvered; } }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GameCrear()
    {
        isGameCleared = true;
    }

    public void GameOver()
    {
        isGameOvered = true;
    }
}
