using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;       //포톤 네트워크 핵심 기능
using Photon.Realtime;  //포톤 서비스 관련(룸 옵션, 디스커넥션 등)

//네트워크 매니저 : 룸(게임 공간)으로 연결시켜주는 역할
//포톤네트워크 : 마스터 서버 -> 로비(대기실) -> 룸(게임 공간)
//MonoBehaviourPunCallbacks : 포톤 서버 접속, 로비 접속, 룸 접속 등 이벤트를 받아올 수 있다.
public class NetworkManager : MonoBehaviourPunCallbacks
{

    public Text infoText;   //네트워크 상태를 보여줄 텍스트
    public Button connectButton;    //룸 접속 버튼

    string gameVersion = "1";   //게임 버전

    private void Awake()
    {
        //해상도 설정
        Screen.SetResolution(800, 600, FullScreenMode.Windowed);
    }

    //네트워크 매니저 실행되면 제일 먼저 할 일?

    // Start is called before the first frame update
    void Start()
    {
        //접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        //마스터 서버에 접속하는 함수 (중요!!)
        PhotonNetwork.ConnectUsingSettings();
        //접속 시도중임을 텍스트로 표시하기
        infoText.text = "마스터 서버에 접속 중...";
        //룸(게임공간) 버튼 비활성화
        connectButton.interactable = false;

    }

    //마스터 서버에 접속을 성공했을 시 자동 실행된다.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //접속 정보 표시하기
        infoText.text = "온라인 : 마스터 서버와 연결 됨";
        //룸(게임공간) 버튼 활성화
        connectButton.interactable = true;

    }

    //혹시 시작하면서 마스터 서버에 접속 실패했을 시 자동 실행된다.
    public override void OnDisconnected(DisconnectCause cause)
    {
        //접속 정보 표시하기
        infoText.text = "오프라인 : 마스터 서버와 연결 실패 \n 접속 재시도";
        //룸(게임공간) 버튼 비활성화
        connectButton.interactable = false;
        //마스터 서버에 접속하는 함수
        PhotonNetwork.ConnectUsingSettings();
    }

    //접속 버튼 클릭 시 이 함수 발동하기
    public void OnConnect()
    {
        //중복 접속 차단하기 접속 버튼 비활성화
        connectButton.interactable = false;

        //마스터 서버에 접속 중인가?
        if (PhotonNetwork.IsConnected)
        {
            //룸(게임 공간)으로 접속 실행
            infoText.text = "랜덤 방에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버에 접속 중이 아니라면 다시 마스터 서버에 접속 시도

            //접속 정보 표시하기
            infoText.text = "오프라인 : 마스터 서버와 연결 실패 \n 접속 재시도";
          
            //마스터 서버에 접속하는 함수
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    //룸에 참가 완료 된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        //접속 정보 표시하기
        infoText.text = "방 참가 성공했음";
        //모든 룸 참가자들이 "GameScene"을 로드함
        PhotonNetwork.LoadLevel("GameScene");
    }

    //(빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 정보 표시하기
        infoText.text = "빈 방이 없으니 새로운 방 생성 중...";
        //빈 방을 생성한다
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }


}
