using UnityEngine;
using UnityEngine.UI;
public class PlayerControls: MonoBehaviour {
    public Text playerStateText;
    PlayerStates playerState = PlayerStates.HoldPosition;
    public PlayerStates getState() {
        return playerState;
    }
    public void setState(PlayerStates state) {
        playerState = state;
        switch(playerState) {
            case PlayerStates.Stop:
            playerStateText.text = "Stop";
            break;
            case PlayerStates.Move:
            playerStateText.text = "Move";
            break;
            case PlayerStates.AttackMove:
            playerStateText.text = "Attack-Move";
            break;
            case PlayerStates.AttackTarget:
            playerStateText.text = "Attack-Target";
            break;
            case PlayerStates.MoveTarget:
            playerStateText.text = "Move-Target";
            break;
            case PlayerStates.HoldPosition:
            default:
            playerStateText.text = "Hold Position";
            break;
        }
    }
}