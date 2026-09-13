// GameConstants.cs - Centralized configuration for the entire game

using UnityEngine;

public static class GameConstants
{
    // Player Settings
    public const float PLAYER_MOVE_SPEED = 10f;
    public const float PLAYER_FORWARD_SPEED = 8f;
    public const float PLAYER_JUMP_FORCE = 5f;
    public const float PLAYER_SLIDE_DURATION = 0.5f;
    public const int NUM_LANES = 3;
    public const float LANE_WIDTH = 2f;

    // Track Generation
    public const float TRACK_SEGMENT_LENGTH = 20f;
    public const int SEGMENTS_AHEAD = 10;
    public const int SEGMENTS_BEHIND = 5;
    public const float TRACK_SCROLL_THRESHOLD = 15f;

    // Obstacles
    public const float OBSTACLE_MIN_SPAWN_INTERVAL = 2f;
    public const float OBSTACLE_MAX_SPAWN_INTERVAL = 5f;
    public const int MAX_OBSTACLES_ACTIVE = 20;

    // Coins
    public const int COIN_POINTS = 10;
    public const float COIN_MIN_SPAWN_INTERVAL = 1.5f;
    public const float COIN_MAX_SPAWN_INTERVAL = 3f;
    public const int MAX_COINS_ACTIVE = 50;

    // Power-ups
    public const float SHIELD_DURATION = 10f;
    public const float MAGNET_DURATION = 8f;
    public const float SPEED_BOOST_DURATION = 5f;
    public const float INVINCIBILITY_DURATION = 8f;
    public const float SPEED_BOOST_MULTIPLIER = 1.5f;

    // Difficulty
    public const int SCORE_FOR_DIFFICULTY_INCREASE = 1000;
    public const float DIFFICULTY_MULTIPLIER = 1.1f; // 10% increase per difficulty level
    public const float MAX_FORWARD_SPEED = 15f;

    // UI
    public const float UI_FADE_DURATION = 0.5f;

    // Physics
    public const float GROUND_DRAG = 5f;
    public const float AIR_DRAG = 1f;
    public const float GRAVITY = -15f;

    // Scoring
    public const int DISTANCE_POINTS_MULTIPLIER = 1; // Points per unit distance
    public const int OBSTACLE_AVOID_POINTS = 50;

    // Layer Names
    public const string LAYER_GROUND = "Ground";
    public const string LAYER_OBSTACLE = "Obstacle";
    public const string LAYER_COIN = "Coin";
    public const string LAYER_POWERUP = "PowerUp";
    public const string LAYER_PLAYER = "Player";
}
