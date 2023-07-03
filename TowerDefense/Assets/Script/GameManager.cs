using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    
    private bool isPlaying = true; //�Q�[�������ǂ����̔���
    private bool isGameCleared = false; //�Q�[���N���A����
    private bool isGameOvered = false; //�Q�[���I�[�o�[����

    private int money = 0; //����
    private int score = 0; //�X�R�A


    //�v���p�e�B�o�R�ŕϐ��̒l���擾����
    public static GameManager Instance { get { return instance; } }
    public bool IsPlaying { get { return isPlaying; } }
    public bool IsGameCleared { get { return isGameCleared; } }
    public bool IsGameOvered { get {  return isGameOvered; } }
    public int Money { get {  return money; } }
    public int Score { get { return score; } }


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