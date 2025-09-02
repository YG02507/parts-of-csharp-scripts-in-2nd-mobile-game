using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

internal sealed partial class PreferenceModule : MonoBehaviour
{
    // Private Structure Definitions

    [System.Serializable]
    private struct Level
    {
        public string LevelDigit;

        public string ProblemsStatus;

        public Level(in string levelDigit, in string problemsStatus)
        {
            LevelDigit = levelDigit;

            ProblemsStatus = problemsStatus;
        }
    }
    [System.Serializable]
    private struct LevelsProgress
    {
        public List<Level> Levels;

        public int LevelUnlockedLimit;

        public LevelsProgress(in int levelUnlockedLimit)
        {
            #region Local Variable Declaration in LevelsProgress Constructor

            StringBuilder problemsStatusSerialData;

            #endregion

            (problemsStatusSerialData = new()).Append((char)Kit.Status.Unsolved);

            for (int i = Kit.ProblemUnitLength.Decrement(); i > default(int); --i)
            {
                problemsStatusSerialData.Append((char)Kit.Status.Locked);
            }

            Levels = new();

            for (int i = default(int).Increment(); i <= Kit.LevelCount; ++i)
            {
                Levels.Add(new(i.DoubleDigit(), problemsStatusSerialData.StringBuilt(false)));
            }

            LevelUnlockedLimit = levelUnlockedLimit;

            problemsStatusSerialData.Clear();
        }
    }

    // Const Private String

    private const string MineralRewardOperandLabelTextFormat = "+  {0}";

    // Const Private Structures

    private const int StandardMineralAmountBoardChildIndexOfPageHeader = 3;
    private const int StandardMineralChildIndexOfBoostingBoard = 1;

    // Readonly Private Collection

    private readonly List<TextMeshProUGUI> MineralAmountLabels = new(3);

    // Readonly Private Classes

    private readonly StringBuilder ProblemsStatusSerialData = new();
    private readonly StringBuilder LabelTexts = new();

    private readonly WaitForSeconds BoostPeriod = new(0.03125f);
    private readonly WaitForSeconds WaitBeforeBoostingPeriod = new(0.5f);

    // Private Components

    [field: SerializeField] private Transform _preferenceModule;    // Parent

    [field: SerializeField] private AudioSource _preferenceModuleAudioSource;

    // Private Class

    [field: SerializeField] private AudioClip _boostingAudioClip;

    // Private Structures

    [field: SerializeField] private int _unitMineralReward;
    [field: SerializeField] private int _cumulatedMineralReward;
}
internal sealed partial class PreferenceModule : MonoBehaviour
{
    // Private Message of MonoBehaviour Class

    private void Awake()
    {
        DebuggingAid.LogDownAboutActivation(this.transform);

        Relate();

        Initialize();
    }
}
internal sealed partial class PreferenceModule : MonoBehaviour
{
    // Private Defined Methods Called at Awake Message

    private void Relate()
    {
        Kit.Relate(out _preferenceModule, this.transform);
        Kit.Relate(out _preferenceModuleAudioSource, _preferenceModule);
    }
    private void Initialize()
    {
        Kit.ExtractClip(out _boostingAudioClip, _preferenceModuleAudioSource);

        _cumulatedMineralReward = default;
    }

    // Private Defined Methods Called at Other Methods

    private void InitializePlayerPreference<T>(in string key, in T value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            SetPlayerPreference(key, value);
        }
    }
    private void SetPlayerPreference<T>(in string key, in T value)
    {
        if (value is LevelsProgress levelsProgress)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(levelsProgress, true));

            DebuggingAid.LogDownAboutData(key, PlayerPrefs.GetString(key));
        }
        else if (value is int @int)
        {
            PlayerPrefs.SetInt(key, @int);

            DebuggingAid.LogDownAboutData(key, PlayerPrefs.GetInt(key));
        }
        else if (value is string @string)
        {
            PlayerPrefs.SetString(key, @string);

            DebuggingAid.LogDownAboutData(key, PlayerPrefs.GetString(key));
        }
    }
    private void TakeLevelsProgressOut(out LevelsProgress levelsProgress)
    {
        levelsProgress = JsonUtility.FromJson<LevelsProgress>(PlayerPrefs.GetString(Kit.LevelsProgressKey));
    }
    private void SettleMineralAmount(in int mineralAmountOperand)
    {
        SetPlayerPreference(Kit.MineralAmountKey, Mathf.Clamp(MineralAmount() + mineralAmountOperand, default, Kit.MineralAmountLimit));

        RewriteAllMineralAmountBoards();
    }
    private string PitKey(in (Kit.Pit Name, int Address) pitData)
    {
        Kit.MakeAnswer(out string answer, pitData.Address.OrdinalText());

        answer += pitData.Name is Kit.Pit.Cosmos ? Kit.CosmosPitKeySuffix : Kit.ChimeraPitKeySuffix;

        return answer;
    }
    private DateTime PitEntryTimestamp(in (Kit.Pit Name, int Address) pitData)
    {
        #region Local Variables Declaration in PitEntryTimestamp Method

        string pitEntryTimestamp;
        string pitEntryTimestampFormat;

        DateTimeStyles pitEntryTimestampStyle;

        #endregion

        pitEntryTimestamp = PlayerPrefs.GetString(PitKey(pitData));

        pitEntryTimestampFormat = Kit.FullDateTimeRepresentationFormatByISO8601;

        pitEntryTimestampStyle = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;

        return DateTime.ParseExact(pitEntryTimestamp, pitEntryTimestampFormat, CultureInfo.InvariantCulture, pitEntryTimestampStyle);
    }
}
internal sealed partial class PreferenceModule : MonoBehaviour
{
    // Internal Defined Methods

    internal void InitializeSaveData()
    {
        InitializePlayerPreference(Kit.LevelsProgressKey, new LevelsProgress(default(int).Increment()));
        InitializePlayerPreference(Kit.MineralAmountKey, Kit.Half(Kit.MineralAmountForUnlockingNextLevel).Form());

        for (int i = Kit.MaximumBlockGroupRank; i > default(int); --i)
        {
            InitializePlayerPreference(PitKey((Kit.Pit.Cosmos, i)), Kit.InitialPitEntryTimestamp);
            InitializePlayerPreference(PitKey((Kit.Pit.Chimera, i)), Kit.InitialPitEntryTimestamp);
        }

        InitializeUnitMineralReward();

        RewriteAllMineralAmountBoards();

        PlayerPrefs.Save();
    }
    internal void UnlockNextProblem((string Level, string Problem) plaintextFileData)
    {
        TakeLevelsProgressOut(out LevelsProgress levelsProgress);

        Kit.MakeIndex(out int levelIndex, plaintextFileData.Level.Form().Index());
        Kit.MakeIndex(out int problemsStatusSerialDataIndex, Mathf.Min(plaintextFileData.Problem.Form()));

        _ = ProblemsStatusSerialData.Clear().Append(levelsProgress.Levels[levelIndex].ProblemsStatus);

        problemsStatusSerialDataIndex = Mathf.Min(problemsStatusSerialDataIndex, Kit.ProblemUnitLength.Decrement().Index());

        if (ProblemsStatusSerialData[problemsStatusSerialDataIndex] is (char)Kit.Status.Locked)
        {
            ProblemsStatusSerialData[problemsStatusSerialDataIndex] = (char)Kit.Status.Unsolved;

            levelsProgress.Levels[levelIndex] = new(plaintextFileData.Level, ProblemsStatusSerialData.StringBuilt(false));

            SetPlayerPreference(Kit.LevelsProgressKey, levelsProgress);

            PlayerPrefs.Save();
        }
    }
    internal void UnlockNextLevelByPayingMineral()
    {
        SettleMineralAmount(-Kit.MineralAmountForUnlockingNextLevel);

        GameManager.Instance.UnlockLevel(LevelUnlockedLimit());

        TakeLevelsProgressOut(out LevelsProgress levelsProgress);

        levelsProgress.LevelUnlockedLimit = Mathf.Min(levelsProgress.LevelUnlockedLimit.Increment(), Kit.LevelCount);

        SetPlayerPreference(Kit.LevelsProgressKey, levelsProgress);

        PlayerPrefs.Save();
    }
    internal int LevelUnlockedLimit()
    {
        TakeLevelsProgressOut(out LevelsProgress levelsProgress);

        return levelsProgress.LevelUnlockedLimit;
    }
    internal string ProblemsStatus(string levelDigit)
    {
        TakeLevelsProgressOut(out LevelsProgress levelsProgress);

        return levelsProgress.Levels.Find(l => l.LevelDigit.Equals(levelDigit)).ProblemsStatus;
    }
    internal int MineralAmount()
    {
        return PlayerPrefs.GetInt(Kit.MineralAmountKey);
    }
    internal void RegisterMineralAmountLabel(RectTransform pageRectTransform)
    {
        Kit.Relate(out Transform mineralAmountBoard, pageRectTransform.Header().Child(StandardMineralAmountBoardChildIndexOfPageHeader));
        Kit.Relate(out TextMeshProUGUI mineralAmountLabelTextMeshProUGUI, mineralAmountBoard.FirstChild());

        MineralAmountLabels.Add(mineralAmountLabelTextMeshProUGUI);
    }
    internal void RewriteAllMineralAmountBoards()
    {
        for (int i = MineralAmountLabels.Count.Index(); i >= default(int); --i)
        {
            Kit.SetText(MineralAmountLabels[i], MineralAmount());
        }
    }
    internal void InitializeUnitMineralReward(in int targetUnitMineralReward = default)
    {
        _unitMineralReward = targetUnitMineralReward;
    }
    internal void IncreaseUnitMineralReward()
    {
        ++_unitMineralReward;
    }
    internal bool IsMineralRewardCumulated()
    {
        return _cumulatedMineralReward > default(int);
    }
    internal void ProcessGameResult(in (string Level, string Problem) plaintextFileData, in Kit.Mode gameMode, in bool isThisGameCleared)
    {
        #region Local Variable Declaration in ProcessGameResult Method

        int clearCount;

        #endregion

        switch (gameMode)
        {
            case Kit.Mode.Cleaning:
                if (isThisGameCleared)
                {
                    TakeLevelsProgressOut(out LevelsProgress levelsProgress);

                    Kit.MakeIndex(out int levelIndex, plaintextFileData.Level.Form().Index());

                    _ = ProblemsStatusSerialData.Clear().Append(levelsProgress.Levels[levelIndex].ProblemsStatus);

                    if (ProblemsStatusSerialData[plaintextFileData.Problem.Form().Index()] is not (char)Kit.Status.Cleared)
                    {
                        ProblemsStatusSerialData[plaintextFileData.Problem.Form().Index()] = (char)Kit.Status.Cleared;

                        clearCount = ProblemsStatusSerialData.StringBuilt(false).Count(c => c is (char)Kit.Status.Cleared);

                        if (clearCount.Equals(Kit.ProblemUnitLength.Decrement()))
                        {
                            ProblemsStatusSerialData[Kit.ProblemUnitLength.Index()] = (char)Kit.Status.Unsolved;
                        }

                        if ((plaintextFileData.Level.Form() < Kit.LevelCount) && (clearCount >= Kit.ClearCountForNextLevelUnlock))
                        {
                            if (plaintextFileData.Level.Form().Equals(levelsProgress.LevelUnlockedLimit))
                            {
                                GameManager.Instance.UnlockLevel(levelsProgress.LevelUnlockedLimit);

                                levelsProgress.LevelUnlockedLimit = Mathf.Min(levelsProgress.LevelUnlockedLimit.Increment(), Kit.LevelCount);
                            }
                        }

                        levelsProgress.Levels[levelIndex] = new(plaintextFileData.Level, ProblemsStatusSerialData.StringBuilt());

                        SetPlayerPreference(Kit.LevelsProgressKey, levelsProgress);

                        if (!plaintextFileData.Problem.Form().Equals(Kit.ProblemUnitLength))
                        {
                            SettleMineralAmount(_unitMineralReward = Kit.BasicMineralRewardOfProblemClear);
                        }
                        else
                        {
                            SettleMineralAmount(_unitMineralReward = Kit.Half(Kit.MineralAmountForUnlockingNextLevel).Form());
                        }

                        _cumulatedMineralReward += _unitMineralReward;
                    }
                    else
                    {
                        InitializeUnitMineralReward();
                    }
                }

                break;
            case Kit.Mode.Mining:
                SettleMineralAmount(_cumulatedMineralReward = _unitMineralReward);

                break;
            default:
                break;
        }

        PlayerPrefs.Save();
    }
    internal void BoostUnitMineralRewardDigit(in TextMeshProUGUI unitMineralRewardLabelTextMeshProUGUI)
    {
        #region Local Variable Declaration in BoostUnitMineralRewardDigit Method

        int boostSpeed;

        #endregion

        boostSpeed = _unitMineralReward.Quotient(Kit.BasicMineralRewardOfProblemClear).Increment();

        StartCoroutine(BoostUnitMineralRewardDigit(unitMineralRewardLabelTextMeshProUGUI, boostSpeed));
    }
    internal void BoostTotalMineralAmountDigit(in TextMeshProUGUI totalMineralAmountLabelTextMeshProUGUI)
    {
        #region Local Variable Declaration in BoostTotalMineralAmountDigit Method

        int boostSpeed;

        #endregion

        boostSpeed = _cumulatedMineralReward.Quotient(Kit.BasicMineralRewardOfProblemClear).Increment();

        StartCoroutine(BoostTotalMineralAmountDigit(totalMineralAmountLabelTextMeshProUGUI, boostSpeed, _cumulatedMineralReward));

        _cumulatedMineralReward = default;
    }
    internal bool IsPitEntryPermitted(in (Kit.Pit Name, int Address) pitData)
    {
        Kit.MakeAnswer(out bool answer, false);

        if (PitEntryWaitingHour(pitData) <= default(int))
        {
            SetPlayerPreference(PitKey(pitData), DateTime.UtcNow.ToString(Kit.FullDateTimeRepresentationFormatByISO8601));

            PlayerPrefs.Save();

            answer = true;
        }

        return answer;
    }
    internal double PitEntryWaitingHour(in (Kit.Pit Name, int Address) pitData)
    {
        return Kit.PitEntryMaximumWaitingHour - (DateTime.UtcNow - PitEntryTimestamp(pitData)).TotalHours;
    }
    internal void ResetPitTimestampByPayingMineral(in (Kit.Pit Name, int Address) pitData)
    {
        SettleMineralAmount(-Kit.MineralAmountForPassingPit);

        SetPlayerPreference(PitKey(pitData), Kit.InitialPitEntryTimestamp);

        PlayerPrefs.Save();
    }
}
internal sealed partial class PreferenceModule : MonoBehaviour
{
    // Private Defined Coroutines

    private IEnumerator BoostUnitMineralRewardDigit(TextMeshProUGUI subjectTextMeshProUGUI, int boostSpeed)
    {
        Kit.Relate(out Animator mineralAnimator, subjectTextMeshProUGUI.transform.parent.Child(StandardMineralChildIndexOfBoostingBoard));

        mineralAnimator.enabled = true;

        for (int i = default; i < _unitMineralReward; i += boostSpeed)
        {
            if (subjectTextMeshProUGUI.transform.gameObject.activeInHierarchy)
            {
                Kit.SetText(subjectTextMeshProUGUI, string.Format(MineralRewardOperandLabelTextFormat, i));

                Kit.Boost(mineralAnimator, _preferenceModuleAudioSource, _boostingAudioClip);
            }
            else
            {
                break;
            }

            yield return BoostPeriod;
        }

        Kit.SetText(subjectTextMeshProUGUI, string.Format(MineralRewardOperandLabelTextFormat, _unitMineralReward));

        mineralAnimator.enabled = false;

        Kit.ScaleLocally(mineralAnimator.transform.Form());

        yield return null;
    }
    private IEnumerator BoostTotalMineralAmountDigit(TextMeshProUGUI subjectTextMeshProUGUI, int boostSpeed, int mineralRewardOperand)
    {
        #region Local Variable Declaration in BoostTotalMineralAmountDigit Method

        int previousTotalMineralAmount;

        #endregion

        Kit.Relate(out Animator mineralAnimator, subjectTextMeshProUGUI.transform.parent.Child(StandardMineralChildIndexOfBoostingBoard));

        _ = LabelTexts.Clear().Append(previousTotalMineralAmount = subjectTextMeshProUGUI.text.Form()).AppendLine();
        _ = LabelTexts.Append(string.Format(MineralRewardOperandLabelTextFormat, mineralRewardOperand));

        Kit.SetText(subjectTextMeshProUGUI, LabelTexts.StringBuilt());

        yield return WaitBeforeBoostingPeriod;

        mineralAnimator.enabled = true;

        for (int i = default; i < mineralRewardOperand; i += boostSpeed)
        {
            if (subjectTextMeshProUGUI.transform.gameObject.activeInHierarchy)
            {
                _ = LabelTexts.Append(Mathf.Min(previousTotalMineralAmount + i, Kit.MineralAmountLimit)).AppendLine();
                _ = LabelTexts.Append(string.Format(MineralRewardOperandLabelTextFormat, mineralRewardOperand - i));

                Kit.SetText(subjectTextMeshProUGUI, LabelTexts.StringBuilt());

                Kit.Boost(mineralAnimator, _preferenceModuleAudioSource, _boostingAudioClip);
            }
            else
            {
                break;
            }

            yield return BoostPeriod;
        }

        _ = LabelTexts.Append(Mathf.Min(previousTotalMineralAmount + mineralRewardOperand, Kit.MineralAmountLimit)).AppendLine();
        _ = LabelTexts.Append(string.Format(MineralRewardOperandLabelTextFormat, default(int)));

        Kit.SetText(subjectTextMeshProUGUI, LabelTexts.StringBuilt());

        mineralAnimator.enabled = false;

        Kit.ScaleLocally(mineralAnimator.transform.Form());

        yield return null;
    }
}
