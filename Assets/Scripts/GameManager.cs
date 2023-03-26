using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public event EventHandler OnStateChanged;
	public event EventHandler OnGamePaused;
	public event EventHandler OnGameUnpaused;
	private enum State
	{
		WaitingToStart,
		CountdownToStart,
		GamePlaying,
		GameOver
	}

	private State state;
	private float countdownToStartTimer = 3f;
	private float gamePlayingTimer;
    private float gamePlayingTimerMax = 120f;
	private bool isGamePaused = false;

    private void Awake()
    {
		Instance = this;
		state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPausedAction += GameInput_OnPausedAction;
        GameInput.Instance.OnUseAction += GameInput_OnUseAction;
    }

    private void Update()
    {
		switch (state)
		{
			case State.CountdownToStart:
				countdownToStartTimer -= Time.deltaTime;
				if (countdownToStartTimer < 0f)
				{
					state = State.GamePlaying;
					gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
				break;
			case State.GamePlaying:
				gamePlayingTimer -= Time.deltaTime;
				if (gamePlayingTimer <0f)
				{
					state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
				break;
			case State.GameOver:
				break;
			default:
				break;
		}
	}

    private void GameInput_OnUseAction(object sender, EventArgs e)
    {
		if (state == State.WaitingToStart)
		{
			state = State.CountdownToStart;
			OnStateChanged?.Invoke(this, EventArgs.Empty);
		}
    }

    private void GameInput_OnPausedAction(object sender, EventArgs e)
    {
		TogglePauseGame();
    }

    public bool IsGamePlaying()
	{
		return state == State.GamePlaying;
	}

    public bool IsCountdownToStart()
    {
		return state == State.CountdownToStart;
    }

	public float GetCountdownToStartTimer()
	{
		return countdownToStartTimer;
	}

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

	public float GetPlayingTimer()
	{
		return 1 - (gamePlayingTimer / gamePlayingTimerMax);
	}

	public void TogglePauseGame()
	{
		isGamePaused = !isGamePaused;
		if (isGamePaused)
		{
			Time.timeScale = 0f;
			OnGamePaused?.Invoke(this, EventArgs.Empty);
		}
		else
		{
            Time.timeScale = 1f;
			OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
	}
}
