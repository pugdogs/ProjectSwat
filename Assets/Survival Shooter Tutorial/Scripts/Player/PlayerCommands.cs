public enum PlayerCommands {
    // player pressed 'a'
    // await target of ground position
    AttackKeypress,

    // player either right-clicked on attackable target or left-clicked following 'a' keypress
    // attack targetted entity
    AttackTarget,

    // player pressed 'm'
    // await target or ground position
    MoveKeypress,

    // player either right-clicked ally or neutral target or left-clicked following 'm' keypress
    // move up to target
    MoveTarget,

    // player left-clicked ground following 'a' keypress
    // acquire next available target while Moving to position
    AttackMove,

    // player right-clicked ground
    //move to position
    Move,

    // 'h' keypress
    //acquire next available target
    HoldPosition,

    // 's' keypress
    //cease movement, do not acquire targets.
    Stop
}