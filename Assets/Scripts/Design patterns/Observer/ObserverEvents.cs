public static class ObserverEvents
{
    #region(Cell and Card)
    public const string CELL_HOVERED = "CELL_HOVERED";
    public const string ACTOR_USED_SKILL = "ACTOR_USED_SKILL";
    public const string CARD_USED = "CARD_USED";
    #endregion

    // #region(Run System)
    // public const string PLAYER_END_TURN = "PLAYER_END_TURN";
    // public const string ENEMY_END_TURN = "ENEMY_END_TURN";
    // #endregion

    #region(Character)
    // public const string CHARACTER_TAKE_DAMAGE = "CHARACTER_TAKE_DAMAGE";
    public const string PLAYER_DEAD = "PLAYER_DEAD";
    public const string PLAYER_REVIVED = "PLAYER_REVIVED";
    #endregion

    #region(Enemy)
    // public const string ENEMY_TAKE_DAMAGE = "ENEMY_TAKE_DAMAGE";
    public const string ENEMY_DEAD = "ENEMY_DEAD";
    #endregion
}