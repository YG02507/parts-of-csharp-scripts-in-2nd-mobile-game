using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal static partial class Kit
{
    // Internal Enumeration Definitions

    internal enum State
    {
        Select = 0,
        Monitor = 1
    }
    internal enum Section
    {
        Header = 0,
        Body = 1,
        Footer = 2
    }
    internal enum Hand
    {
        Left = 0,
        Right = 1
    }
    internal enum Design
    {
        Simple = 0,
        Theme = 1
    }
    internal enum Mode
    {
        Cleaning = 0,
        Mining = 1
    }
    internal enum Pit
    {
        Cosmos = 0,
        Chimera = 1
    }
    internal enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
    internal enum Occupancy
    {
        Empty = '-',
        Terrain = 'X',
        Stone = 'S'
    }
    internal enum Gravity
    {
        Up = 'U',
        Right = 'R',
        Down = 'D',
        Left = 'L'
    };
    internal enum Status
    {
        Locked = '-',
        Unsolved = 'X',
        Cleared = 'O'
    }

    // Const Internal Strings

    internal const string PrefabsDirectoryPathPrefixInResourcesDirectory = "Prefabs/";
    internal const string PlaintextsDirectoryPathPrefixInResourcesDirectory = "Plaintexts/";
    internal const string FormationsDirectoryPathPrefixInResourcesDirectory = "Formations/";
    internal const string SpritesDirectoryPathPrefixInResourcesDirectory = "Sprites/";
    internal const string BlocksDirectoryPathPrefixInSpritesDirectory = "Blocks/";
    internal const string TerrainsDirectoryPathPrefixInSpritesDirectory = "Terrains/";
    internal const string AirsDirectoryPathPrefixInSpritesDirectory = "Airs/";
    internal const string AudiosDirectoryPathPrefixInResourcesDirectory = "Audios/";
    internal const string LoopsDirectoryPathPrefixInAudiosDirectory = "Loops/";
    internal const string LevelsProgressKey = "Levels Progress";
    internal const string MineralAmountKey = "Mineral";
    internal const string CosmosPitKeySuffix = " Cosmos Pit";
    internal const string ChimeraPitKeySuffix = " Chimera Pit";
    internal const string FullDateTimeRepresentationFormatByISO8601 = "o";
    internal const string InitialPitEntryTimestamp = "2021-01-01T01:01:01.1111111Z";
    internal const string PlaintextFileNamePrefix = "plaintext";
    internal const string FormationFileNamePrefix = "formation";
    internal const string BlockText = "Block";
    internal const string TerrainText = "Terrain";
    internal const string AirText = "Air";
    internal const string GravitationalForceButtonName = "Gravitational Force Button";
    internal const string ClassicalForceButtonName = "Classical Force Button";
    internal const string ClassicalForceName = "Classical Force";
    internal const string ActivatedGravitationalForceName = "Activated Gravitational Force";
    internal const string DeactivatedGravitationalForceName = "Deactivated Gravitational Force";
    internal const string BeepingAudioName = "Beeping";
    internal const string PoppingAudioName = "Popping";

    // Const Internal Structures

    internal const float FixedMoveSpeed = 5.0f;
    internal const float MagnitudeOfMoveAcceleration = 30.0f;
    internal const float RightAngleDegree = 90.0f;
    internal const float AreaEdgeWidth = 1.0f;

    internal const double PitEntryMaximumWaitingHour = 2.0;

    internal const int StandardApplicationTargetFrameRate = 120;
    internal const int FullPercentage = 100;
    internal const int MaximumBlockSortCount = 10;
    internal const int MaximumBowlMatrixCapacity = 100;
    internal const int MinimumForcePower = 1;
    internal const int MaximumForcePower = 9;
    internal const int ForceStockMaximumLimit = 99;
    internal const int DeadIndex = -1;
    internal const int LevelCount = 20;
    internal const int ProblemUnitLength = 25;
    internal const int ClearCountForNextLevelUnlock = 20;
    internal const int MaximumBlockGroupRank = 4;
    internal const int PitMinimumInternalSize = 70;
    internal const int MineralAmountLimit = 9999;
    internal const int MineralAmountForUnlockingNextLevel = 500;
    internal const int MineralAmountForPassingPit = 100;
    internal const int BasicMineralRewardOfProblemClear = 20;

    internal const char InitialDeactivatedGravitationalForceDeterminant = 'G';
    internal const char EndOfPlaintextFileHeaderMark = '!';
    internal const char EndOfPlaintextDataUnitMark = 'C';
    internal const char SafeEscapeFromReadingPlaintextFileMark = 'R';
    internal const char IdentifierHyphen = '-';
}
internal static partial class Kit
{
    // Static Internal Defined Methods

    internal static GameObject LoadedPrefab(in string prefabFilePathSuffix)
    {
        return Resources.Load<GameObject>(PrefabsDirectoryPathPrefixInResourcesDirectory + prefabFilePathSuffix);
    }
    internal static TextAsset LoadedPlaintext(in string plaintextFilePathSuffix)
    {
        return Resources.Load<TextAsset>(PlaintextsDirectoryPathPrefixInResourcesDirectory + plaintextFilePathSuffix);
    }
    internal static TextAsset LoadedFormation(in string formationFilePathSuffix)
    {
        return Resources.Load<TextAsset>(FormationsDirectoryPathPrefixInResourcesDirectory + formationFilePathSuffix);
    }
    internal static Sprite LoadedSprite(in string spriteFilePathSuffix)
    {
        return Resources.Load<Sprite>(SpritesDirectoryPathPrefixInResourcesDirectory + spriteFilePathSuffix);
    }
    internal static AudioClip LoadedAudio(in string audioFilePathSuffix)
    {
        return Resources.Load<AudioClip>(AudiosDirectoryPathPrefixInResourcesDirectory + audioFilePathSuffix);
    }
    internal static float ScreenRatio()
    {
        return (float)Screen.width * ((float)Screen.height).Reciprocal();
    }
    internal static bool IsScreenResolutionChange(in (int Width, int Height) previousScreenPixels)
    {
        return (!Screen.width.Equals(previousScreenPixels.Width)) || (!Screen.height.Equals(previousScreenPixels.Height));
    }
    internal static float Half(in int subjectInteger = 1)
    {
        return 0.5f * (float)subjectInteger;
    }
    internal static bool IsEmpty(in int subjectCount)
    {
        return subjectCount <= default(int);
    }
    internal static void Rectify(ref float subjectFloat)
    {
        subjectFloat = Mathf.Max(subjectFloat, default);
    }
    internal static void WriteOnMatrix<T>(in T[,] subjectTMatrix, in (int Row, int Column) matrixIndexes, in T element)
    {
        subjectTMatrix[matrixIndexes.Row, matrixIndexes.Column] = element;
    }
    internal static bool RandomBool()
    {
        return (_ = new System.Random()).Next(2).Equals(default);
    }
    internal static void FisherYatesShuffle<T>(in List<T> subjectList, in System.Random randomForShuffle)
    {
        #region Local Variables Declaration in FisherYatesShuffle Method

        T pickedK;

        int k;

        #endregion

        for (int i = subjectList.Count.Index(); i > default(int); --i)
        {
            pickedK = subjectList[k = randomForShuffle.Next(i.Increment())];

            subjectList[k] = subjectList[i];

            subjectList[i] = pickedK;
        }
    }
    internal static T SelectedDataRandomly<T>(in int probabilityIndex, in List<(T Data, float?[] Probabilities)> dataProbabilities)
    {
        #region Local Variables Declaration in SelectedDataRandomly Method

        float randomValue;
        float cumulatedValue;

        #endregion

        randomValue = UnityEngine.Random.Range(default, (float)FullPercentage);

        cumulatedValue = default;

        MakeAnswer(out T answer, dataProbabilities[default].Data);

        for (int i = default; i < dataProbabilities.Count; ++i)
        {
            if (dataProbabilities[i].Probabilities[probabilityIndex].HasValue)
            {
                if (randomValue <= (cumulatedValue += (float)FullPercentage * dataProbabilities[i].Probabilities[probabilityIndex].Value))
                {
                    answer = dataProbabilities[i].Data;

                    break;
                }
            }
            else
            {
                answer = dataProbabilities[Mathf.Max(i.Decrement(), default)].Data;

                break;
            }
        }

        return answer;
    }
    internal static bool IsClickedButtonFound(out Transform foundClickedButton)
    {
        MakeAnswer(out bool answer, EventSystem.current != null);

        foundClickedButton = answer ? EventSystem.current.currentSelectedGameObject.transform : null;

        return answer;
    }
    internal static bool IsButtonLocked(in Transform subjectButton)
    {
        return !subjectButton.GetComponent<Animator>().enabled;
    }
    internal static void SetExtendedButtonWithEventTrigger(in Transform subjectTransform, in bool isEnabled)
    {
        subjectTransform.GetComponent<EventTrigger>().enabled = subjectTransform.GetComponent<ExtendedButton>().interactable = isEnabled;

        if (!(subjectTransform.GetComponent<Animator>().enabled = isEnabled))
        {
            subjectTransform.GetComponent<Image>().color = Color.white;
        }
    }
    internal static void SetButtonLock(in bool isButtonLocked, in Transform subjectButton)
    {
        if (subjectButton.Child(subjectButton.childCount.Index()).gameObject.name.Equals("Button Lock"))
        {
            if (!(subjectButton.GetComponent<Animator>().enabled = !isButtonLocked))
            {
                subjectButton.GetComponent<Image>().color = Color.black;
            }

            SetGameObjectActive(isButtonLocked, subjectButton.Child(subjectButton.childCount.Index()));
        }
        else
        {
            DebuggingAid.WarnPreconditionBreach("The Button Lock must be the last sibling and have the correct name.");
        }
    }
    internal static void SetText<T>(in RectTransform subjectRectTransform, in T literalValue)
    {
        if (subjectRectTransform.FirstChild().TryGetComponent(out TextMeshProUGUI subjectTextMeshProUGUI))
        {
            subjectTextMeshProUGUI.text = literalValue.ToString();
        }
        else
        {
            DebuggingAid.WarnPreconditionBreach("The Label must be the first sibling.");
        }
    }
    internal static void SetText<T>(in TextMeshProUGUI subjectTextMeshProUGUI, in T literalValue)
    {
        subjectTextMeshProUGUI.text = literalValue.ToString();
    }
    internal static void SetGameObjectActive(in bool isActivated, in Transform subjectTransform)
    {
        subjectTransform.gameObject.SetActive(isActivated);
    }
    internal static void SetGameObjectActive(in bool isActivated, in RectTransform subjectRectTransform)
    {
        subjectRectTransform.transform.gameObject.SetActive(isActivated);
    }
    internal static void PutLocally(in Transform subjectTransform, in (float X, float Y) putPlacement)
    {
        subjectTransform.localPosition = putPlacement.X * Vector3.right + putPlacement.Y * Vector3.up;
    }
    internal static void PutLocally(in Transform subjectTransform, in Vector2 putVector)
    {
        subjectTransform.localPosition = putVector.x * Vector3.right + putVector.y * Vector3.up;
    }
    internal static void Put(in Transform subjectTransform, in Vector3 putVector)
    {
        subjectTransform.position = putVector;
    }
    internal static void RotateLocally(in Transform subjectTransform, in float rotateDegree)
    {
        subjectTransform.localRotation = Quaternion.Euler(rotateDegree * Vector3.back);
    }
    internal static void ScaleLocally(in RectTransform subjectRectTransform, in float scaleScalar = 1.0f)
    {
        subjectRectTransform.localScale = scaleScalar * (Vector3.right + Vector3.up) + Vector3.forward;
    }
    internal static void ScaleLocally(in Transform subjectTransform, in float scaleScalar = 1.0f)
    {
        subjectTransform.localScale = scaleScalar * (Vector3.right + Vector3.up) + Vector3.forward;
    }
    internal static void ScaleLocally(in Transform subjectTransform, in Vector2 scaleVector)
    {
        subjectTransform.localScale = scaleVector.x * Vector3.right + scaleVector.y * Vector3.up + Vector3.forward;
    }
    internal static void TranslateScaleSizeLocally(in Transform subjectTransform, in float scaleSize)
    {
        subjectTransform.localScale += scaleSize * (Vector3.right + Vector3.up);
    }
    internal static void Relate(out Transform transformRelated, in Transform rootTransform, params int[] childIndexes)
    {
        transformRelated = rootTransform.Child(childIndexes);
    }
    internal static void Relate(out RectTransform rectTransformRelated, in RectTransform rootRectTransform, params int[] childIndexes)
    {
        rectTransformRelated = rootRectTransform.transform.Child(childIndexes).Form();
    }
    internal static void Relate<T>(out T componentRelated, in Transform rootTransform) where T : Component
    {
        componentRelated = rootTransform.GetComponent<T>();
    }
    internal static void Relate<T>(out T componentRelated, in RectTransform rootRectTransform) where T : Component
    {
        componentRelated = rootRectTransform.transform.GetComponent<T>();
    }
    internal static void BecomeChild(in Transform subjectChildTransform, in Transform targetParentTransform)
    {
        subjectChildTransform.parent = targetParentTransform;

        subjectChildTransform.SetAsLastSibling();
    }
    internal static void SetSpriteRendererSprite(in Transform subjectTransform, in Sprite targetSprite)
    {
        subjectTransform.GetComponent<SpriteRenderer>().sprite = targetSprite;
    }
    internal static void SetSpriteRendererSprite(in SpriteRenderer subjectSpriteRenderer, in string spriteFilePathSuffix)
    {
        subjectSpriteRenderer.sprite = LoadedSprite(spriteFilePathSuffix);
    }
    internal static void SetSpriteRendererSize(in SpriteRenderer subjectSpriteRenderer, in Vector2 sizeVector)
    {
        subjectSpriteRenderer.size = sizeVector;
    }
    internal static void ExtractClip(out AudioClip subjectAudioClip, in AudioSource subjectAudioSource)
    {
        subjectAudioClip = subjectAudioSource.clip;
    }
    internal static void MakeAnswer<T>(out T answerMade, in T initialAnswer)
    {
        answerMade = initialAnswer;
    }
    internal static void MakeIndex(out int indexMade, in int initialIndex = default)
    {
        indexMade = initialIndex;
    }
    internal static void MakeVector(out Vector2 vectorMade, in float initialXCoordinate = default, in float initialYCoordinate = default)
    {
        vectorMade = initialXCoordinate * Vector2.right + initialYCoordinate * Vector2.up;
    }
    internal static void MakeVector(out Vector3 vectorMade, in Vector3 initialVector)
    {
        vectorMade = initialVector;
    }
    internal static void MakeParticleSystemModule<T>(out T particleSystemModuleMade, in T initialParticleSystemModule)
    {
        particleSystemModuleMade = initialParticleSystemModule;
    }
    internal static Color WhiteWithAlpha(float whiteAlpha)
    {
        return new Color(Vector3.one.x, Vector3.one.y, Vector3.one.z, whiteAlpha);
    }
    internal static IEnumerable<char> FullTerrainOccupancyLine()
    {
        return Enumerable.Repeat((char)Occupancy.Terrain, MaximumForcePower.Increment());
    }
    internal static void OpenPage(in RectTransform pageRectTransform, in bool isAnimated, in Action actForRefreshing = null)
    {
        actForRefreshing?.Invoke();

        if (isAnimated)
        {
            pageRectTransform.Header().anchorMin = Vector2.zero + Vector2.left + pageRectTransform.Header().anchorMin.y * Vector2.up;

            pageRectTransform.Header().anchorMax = Vector2.right + Vector2.left + pageRectTransform.Header().anchorMax.y * Vector2.up;

            pageRectTransform.Body().anchorMin = Vector2.zero + Vector2.right + pageRectTransform.Body().anchorMin.y * Vector2.up;

            pageRectTransform.Body().anchorMax = Vector2.right + Vector2.right + pageRectTransform.Body().anchorMax.y * Vector2.up;

            pageRectTransform.Footer().anchorMin = Vector2.zero + Vector2.left + pageRectTransform.Footer().anchorMin.y * Vector2.up;

            pageRectTransform.Footer().anchorMax = Vector2.right + Vector2.left + pageRectTransform.Footer().anchorMax.y * Vector2.up;

            pageRectTransform.Header().GetComponent<Animator>().enabled = true;

            pageRectTransform.Body().GetComponent<Animator>().enabled = true;

            pageRectTransform.Footer().GetComponent<Animator>().enabled = true;
        }

        SetGameObjectActive(true, pageRectTransform);
    }
    internal static void ClosePage(in RectTransform pageRectTransform, in bool isAnimatorIn)
    {
        if (isAnimatorIn)
        {
            pageRectTransform.Header().GetComponent<Animator>().enabled = false;

            pageRectTransform.Body().GetComponent<Animator>().enabled = false;

            pageRectTransform.Footer().GetComponent<Animator>().enabled = false;
        }

        SetGameObjectActive(false, pageRectTransform);
    }
    internal static void DecodePitIdentifierArgument(in int pitIdentifierArgument, ref (Pit Name, int Address) pitData)
    {
        pitData = ((Pit)(pitIdentifierArgument % 10), pitIdentifierArgument.Quotient(10));
    }
    internal static int UnitBlockCountInGroup()
    {
        return LevelCount.Quotient(MaximumBlockGroupRank);
    }
    internal static void MoveToCenter(in (int Row, int Column) maximumBlockLineLengths, params Transform[] subjectTransforms)
    {
        #region Local Variable Declaration in MoveToCenter Method

        Vector2 centerPoint;

        #endregion

        centerPoint.x = (maximumBlockLineLengths.Column % 2).Equals(default) ? -Half() : default;
        centerPoint.y = (maximumBlockLineLengths.Row % 2).Equals(default) ? Half() : default;

        for (int i = subjectTransforms.Length.Index(); i >= default(int); --i)
        {
            PutLocally(subjectTransforms[i], centerPoint);
        }
    }
    internal static bool IsGravitationalForce(in int forcePower)
    {
        return forcePower > MaximumForcePower;
    }
    internal static void Boost(in Animator subjectAnimator, in AudioSource subjectAudioSource, in AudioClip boostingAudioClip)
    {
        subjectAnimator.Play("Mineral Boost", default, default);

        subjectAudioSource.PlayOneShot(boostingAudioClip);
    }

    // Static Internal Defined Extension Methods

    internal static RectTransform Form(this Transform thisTransform)
    {
        return thisTransform.GetComponent<RectTransform>();
    }
    internal static Vector3 Form(this Vector2 thisVector2)
    {
        return thisVector2.x * Vector3.right + thisVector2.y * Vector3.up;
    }
    internal static Vector2 Form(this Vector3 thisVector3)
    {
        return thisVector3.x * Vector2.right + thisVector3.y * Vector2.up;
    }
    internal static int Form(this string thisString)
    {
        return int.Parse(thisString, CultureInfo.InvariantCulture);
    }
    internal static int Form(this float thisFloat)
    {
        return Mathf.RoundToInt(thisFloat);
    }
    internal static char Form(this int thisInt)
    {
        return (char)(thisInt + '0');
    }
    internal static int Form(this char thisChar)
    {
        return thisChar - '0';
    }
    internal static Vector2 Form(this Direction? thisNullableDirection)
    {
        return thisNullableDirection switch
        {
            null => Vector2.zero,
            Direction.Up => Vector2.up,
            Direction.Right => Vector2.right,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            _ => Vector2.zero
        };
    }
    internal static Vector2Int Form(this (int, int) thisIntIntTuple)
    {
        return thisIntIntTuple.Item2 * Vector2Int.right + thisIntIntTuple.Item1 * Vector2Int.up;
    }
    internal static (int Row, int Column) Form(this Vector2Int thisVector2Int)
    {
        return (thisVector2Int.y, thisVector2Int.x);
    }
    internal static Vector2 DoubleVector(this Vector2 thisVector2)
    {
        return thisVector2 + thisVector2;
    }
    internal static Vector3 DoubleVector(this Vector3 thisVector3)
    {
        return thisVector3 + thisVector3;
    }
    internal static float Half(this float thisFloat)
    {
        return 0.5f * thisFloat;
    }
    internal static int Quotient(this int thisInt, in int divisor = 2)
    {
        return thisInt / divisor;
    }
    internal static float Reciprocal(this float thisFloat)
    {
        return 1.0f / thisFloat;
    }
    internal static int Increment(this int thisInt)
    {
        return ++thisInt;
    }
    internal static int Decrement(this int thisInt)
    {
        return --thisInt;
    }
    internal static int Index(this int thisInt)
    {
        return thisInt.Decrement();
    }
    internal static T Last<T>(this T[] thisTArray)
    {
        return thisTArray[thisTArray.Length.Index()];
    }
    internal static T Element<T>(this T[,] thisTTwoDimensionalArray, in (int Row, int Column) matrixIndexes)
    {
        return thisTTwoDimensionalArray[matrixIndexes.Row, matrixIndexes.Column];
    }
    internal static string DoubleDigit(this int thisInt)
    {
        return string.Format("{0:D2}", thisInt);
    }
    internal static string OrdinalText(this int thisInt)
    {
        return thisInt switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => thisInt.ToString() + "th"
        };
    }
    internal static string LoadingLabelText(this int thisInt)
    {
        return "Loading..." + Environment.NewLine + Mathf.Min(thisInt, FullPercentage) + "%";
    }
    internal static string PlaintextFileName(this (string, string) thisStringStringTuple)
    {
        return PlaintextFileNamePrefix + thisStringStringTuple.Item1 + thisStringStringTuple.Item2;
    }
    internal static (string Level, string Problem) NextPlaintextFileData(this (string, string) thisStringStringTuple)
    {
        MakeAnswer(out (string Level, string Problem) answer, thisStringStringTuple);

        if (answer.Problem.Form().Increment() > ProblemUnitLength)
        {
            answer = (Mathf.Min(answer.Level.Form().Increment(), LevelCount).DoubleDigit(), default(int).Increment().DoubleDigit());
        }
        else
        {
            answer.Problem = answer.Problem.Form().Increment().DoubleDigit();
        }

        return answer;
    }
    internal static string CleaningModeDesignatorBoardText(this (string, string) thisStringStringTuple)
    {
        return string.Format("Level {0}-{1}", thisStringStringTuple.Item1, thisStringStringTuple.Item2);
    }
    internal static string StringBuilt(this StringBuilder thisStringBuilder, in bool isClearing = true)
    {
        MakeAnswer(out string answer, thisStringBuilder.ToString());

        if (isClearing)
        {
            thisStringBuilder.Clear();
        }

        return answer;
    }
    internal static int EnumerationLength(this Type thisType)
    {
        return Enum.GetNames(thisType).Length;
    }
    internal static void StopCoroutineSafely(this MonoBehaviour thisMonoBehaviour, in IEnumerator targetCoroutine)
    {
        if (targetCoroutine != null)
        {
            thisMonoBehaviour.StopCoroutine(targetCoroutine);
        }
    }
    internal static string ButtonDigits(this Transform thisTransform)
    {
        MakeAnswer(out string answer, default(int).Increment().DoubleDigit());

        if (thisTransform.FirstChild().TryGetComponent(out TextMeshProUGUI subjectTextMeshProUGUI))
        {
            if (subjectTextMeshProUGUI.gameObject.name[^"Number Label".Length..].Equals("Number Label"))
            {
                answer = subjectTextMeshProUGUI.text[^default(int).DoubleDigit().Length..];
            }
            else
            {
                DebuggingAid.WarnPreconditionBreach("The Number Label about the Digits must be the first sibling.");
            }
        }
        else
        {
            DebuggingAid.WarnPreconditionBreach("The Number Label about the Digits must be the first sibling.");
        }

        return answer;
    }
    internal static RectTransform Header(this RectTransform thisRectTransform)
    {
        return thisRectTransform.transform.Child((int)Section.Header).Form();
    }
    internal static RectTransform Body(this RectTransform thisRectTransform)
    {
        return thisRectTransform.transform.Child((int)Section.Body).Form();
    }
    internal static RectTransform Footer(this RectTransform thisRectTransform)
    {
        return thisRectTransform.transform.Child((int)Section.Footer).Form();
    }
    internal static Transform Child(this Transform thisTransform, params int[] childIndexes)
    {
        MakeAnswer(out Transform answer, thisTransform);

        for (int i = default; i < childIndexes.Length; ++i)
        {
            answer = answer.GetChild(childIndexes[i]);
        }

        return answer;
    }
    internal static Transform Child(this RectTransform thisRectTransform, params int[] childIndexes)
    {
        MakeAnswer(out Transform answer, thisRectTransform.transform.Child(childIndexes));

        return answer;
    }
    internal static Transform FirstChild(this Transform thisTransform)
    {
        MakeAnswer(out Transform answer, thisTransform.GetChild(default));

        return answer;
    }
    internal static string PlaintextFileNameSuffix(this string thisString)
    {
        return thisString[PlaintextFileNamePrefix.Length..];
    }
    internal static string LevelDigit(this string thisString)
    {
        return thisString[PlaintextFileNamePrefix.Length..(PlaintextFileNamePrefix.Length + default(int).DoubleDigit().Length)];
    }
    internal static (int Row, int Column) MatrixIndexes(this Vector3 thisVector3, in (int Row, int Column) maximumBlockLineLengths)
    {
        MakeAnswer(out (int Row, int Column) answer, (maximumBlockLineLengths.Row, maximumBlockLineLengths.Column));

        answer = ((Mathf.Floor(Half(answer.Row)) - thisVector3.y).Form(), (Mathf.Floor(Half(answer.Column)) + thisVector3.x).Form());

        return answer;
    }
    internal static T Choice<T>(this (T, T) thisTTTuple, in Func<T, T, T> actForComparing)
    {
        return actForComparing(thisTTTuple.Item1, thisTTTuple.Item2);
    }
}

// Internal Defined Interfaces

internal interface IWritable<T>
{
    void Write(in T valueWritten);
}
internal interface IContract
{
    void ContractWriting(in GameManager gameManagerInstance);
}
internal interface IRemains
{
    void Register(in List<IRemains> blockRemains, in Kit.Mode gameMode);
    void Activate(in GameObject block);
    void Dispose();
}
