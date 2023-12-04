using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class TypingText
{
    public string japaneseText;
    public string hiraganaText;

    public TypingText(string japaneseText, string hiraganaText)
    {
        this.japaneseText = japaneseText;
        this.hiraganaText = hiraganaText;
    }
}

public class InputLetter : MonoBehaviour
{
    //画面表示するTextMeshPro
    public TextMeshProUGUI japaneseText;
    public TextMeshProUGUI hiraganaText;
    public TextMeshProUGUI alphabetText;
    public TextMeshProUGUI clearText;

    TypingText nowTypingText;
    int nowHiraganaIndex;
    int nowAlphabetIndex;
    int nowTypingLetterIndex;
    int nowTextIndex;
    bool isClear;
    string displayText;
    char nowHiraganaLetter;
    List<TypingText> TypingTexts;

    List<string> alphabetStrings;
    int selectAlphabetIndex;
    char alphabetLetter;

    bool keyPushFlag;

    KeyCode keycode1;
    KeyCode keycode2;

    string replaceAlphabet;
    int replaceAlphabetIndex1;
    int replaceAlphabetIndex2;


    List<char> tuNextHiraganaCheckList = new List<char> { 'あ', 'い', 'う', 'え', 'お', 'っ'};
    List<char> nNextHiraganaCheckList = new List<char> { 'あ', 'い', 'う', 'え', 'お', 'な', 'に', 'ぬ', 'ね', 'の', 'や', 'ゆ', 'よ', 'ん' };

    List<char> firstAlphabetCheckList = new List<Char> { 'か', 'く', 'こ', 'せ', 'ち', 'ふ', 'じ', 'ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', 'ゃ', 'ゅ', 'ょ' };

    //BGM, SE
    public AudioClip typingSoundA1;
    public AudioClip typingSoundA2;
    public AudioClip typingSoundA3;
    public AudioClip typingSoundA4;
    public AudioClip typingSoundA5;
    public AudioClip typingSoundA6;

    public AudioClip typingSoundB1;
    public AudioClip typingSoundB2;
    public AudioClip typingSoundB3;

    public AudioClip typingSoundC1;
    public AudioClip typingSoundC2;
    public AudioClip typingSoundC3;

    public AudioClip typingSoundD1;
    public AudioClip typingSoundD2;
    public AudioClip typingSoundD3;

    public AudioClip typingSoundE1;
    public AudioClip typingSoundE2;
    public AudioClip typingSoundE3;

    public AudioClip typingSoundF1;
    public AudioClip typingSoundF2;
    public AudioClip typingSoundF3;

    public AudioClip typingSoundG;

    public AudioClip missSound;

    AudioSource audioSource;
    public GameObject mainCamera;

    public int typingSound = 0;

    //ミスタイプ時のエフェクト
    public GameObject missRedPanel;
    Image missRedImage;


    //キーボード入力表（普通じゃない打ち方メモ）（なにかに使えるかも
    //Done
    //ん：n, nn, xn

    //打ち方が次の平仮名に関わらない
    //1文字目で打ち方が確定
    //か、く、こ：(ka,ca), (ku,cu,qu),(ko,co)
    //せ：se,ce
    //ち：ti,chi
    //ふ：hu,fu
    //じ：zi,ji
    //ぁ、ぃ、ぅ、ぇ、ぉ：(la,xa),(li,xi),(lu,xu),(le,xe),(lo,xo)
    //ゃ、ゅ、ょ：(lya, xya),(lyu,xyu),(lyo,xyo)

    //2文字目で打ち方が確定
    //う：u,wu,whu
    //し：si,shi,ci
    //つ：tu,tsu



    //打ち方が次の平仮名に影響する
    //拗音がや行（ya,yi,yu,ye,yo)
    //きゃ、きぃ、きゅ、きぇ、きょ：kya,kyi,kyu,kye,kyo
    //ぎゃ、ぎぃ、ぎゅ、ぎえ、ぎょ：gya,gyi,gyu,gye,gyo
    //ぢゃ、ぢぃ、ぢゅ、ぢぇ、ぢょ：dya,dyi,dyu,dye,dyo
    //てゃ、てぃ、てゅ、てぇ、てょ：tha,thi,thu,the,tho
    //でゃ、でぃ、でゅ、でぇ、でょ：dha,dhi,dhu,dhe,dho
    //にゃ、にぃ、にゅ、にぇ、にょ：nya,nyi,nyu,nye,nyo
    //ひゃ、ひぃ、ひゅ、ひぇ、ひょ：hya,hyi,hyu,hye,hyo
    //びゃ、びぃ、びゅ、びぇ、びょ：bya,byi,byu,bye,byo
    //ぴゃ、ぴぃ、ぴゅ、ぴぇ、ぴょ：pya,pyi,pyu,pye,pyo
    //ゔゃ、ゔぃ、ゔゅ、ゔぇ、ゔょ：vya,vyi,vyu,vye,vyo
    //ふゃ、ふぃ、ふゅ、ふぇ、ひょ：fya,fyi,fyu,fye,fyo
    //みゃ、みぃ、みゅ、みぇ、みょ：mya,myi,myu,mye,myo
    //りゃ、りぃ、りゅ、りぇ、りょ：rya,ryi,ryu,rye,ryo

    //拗音がや行（ya,yi,yu,ye,yo + α）
    //しゃ、しぃ、しゅ、しぇ、しょ：(sya,sha),syi,(syu,shu),(sye,she),(syo,sho)
    //じゃ、じぃ、じゅ、じぇ、じょ：(jya,zya,ja),jyi,(jyu,zyu,ju),(jye,zye,je),(jyo,zyo,jo)
    //ちゃ、ちぃ、ちゅ、ちぇ、ちょ：(tya,cya,cha),(tyi,cyi),(tyu,cyu,chu),(tye,cye,che),(tyo,cyo,cho)

    //拗音があ行(wa,wi,wu,we,wo)
    //すぁ、すぃ、すぅ、すぇ、すぉ：swa,swi,swu,swe,swo
    //とぁ、とぃ、とぅ、とぇ、とぉ：twa,twi,twu,twe,two
    //どぁ、どぃ、どぅ、どぇ、どぉ：dwa,dwi,dwu,dwe,dwo
    //ぐぁ、ぐぃ、ぐぅ、ぐぇ、ぐぉ：gwa,gwi,gwu,gwe,gwo

    //拗音があ行(その他)
    //うぁ、うぃ、うぇ、うぉ：wha,whi,whe,who
    //ゔぁ、ゔぃ、ゔ、ゔぇ、ゔぉ：va,vi,vu,ve,vo
    //くぁ、くぃ、くぇ、くぉ：(qa,kwa,qwa),(qi,kwi),(kwu),(qe,kwe),(qo,kwo)
    //つぁ、つぃ、　　　つぇ、つぉ：tsa,tsi,tse,tso
    //ふぁ、ふぃ、　　　ふぇ、ふぉ：fa,fi,fe,fo


    public List<TypingText> TextDataBase = new List<TypingText>
    {
        //new TypingText("あっか、んや、んあ、んにゃ、きゃ、しゃ、ちゃ、にゃ、びゃ、みゃ", "あっか、んや、んあ、んにゃ、きゃ、しゃ、ちゃ、にゃ、びゃ、みゃ"),
        //new TypingText("んか(nn)", "んか"),
        //new TypingText("んか(xn)", "んか"),
        //new TypingText("んか(n)", "んか"),
        //new TypingText("んあ(nn)", "んあ"),
        //new TypingText("んあ(xn)", "んあ"),
        //new TypingText("んな(nn)", "んな"),
        //new TypingText("んな(xn)", "んな"),
        //new TypingText("んん(nn, nn)", "んん"),
        //new TypingText("んん(nn, xn)", "んん"),
        //new TypingText("んん(xn, nn)", "んん"),
        //new TypingText("んん(xn, xn)", "んん"),
        //new TypingText("んや(xn)", "んや"),
        //new TypingText("んや(ln)", "んや"),


        //new TypingText("タッチ(tti)", "たっち"),
        //new TypingText("タッチ(ltu)", "たっち"),
        //new TypingText("タッチ(xtu)", "たっち"),
        //new TypingText("タッ(ltu)", "たっ"),
        //new TypingText("タッ(xtu)", "たっ"),
        //new TypingText("あっあ(ltu)", "あっあ"),
        //new TypingText("あっあ(xtu)", "あっあ"),
        //new TypingText("あっっ(ltu, ltu)", "あっっ"),
        //new TypingText("あっっ(ltu, xtu)", "あっっ"),
        //new TypingText("あっっ(xtu, ltu)", "あっっ"),
        //new TypingText("あっっ(xtu, xtu)", "あっっ")

        //new TypingText("ち（ti）", "ち"),
        //new TypingText("ち（chi）", "ち"),
        //new TypingText("かくこ（ka）", "かくこ"),
        //new TypingText("かくこ（ca）", "かくこ"),
        //new TypingText("せ（se）", "せ"),
        //new TypingText("せ（ce）", "せ"),
        
        //new TypingText("ふ（hu）", "ふ"),
        //new TypingText("ふ（fu）", "ふ"),
        //new TypingText("じ（zi）", "じ"),
        //new TypingText("じ（ji）", "じ"),
        //new TypingText("ぁぃぅぇぉ（l）", "ぁぃぅぇぉ"),
        //new TypingText("ぁぃぅぇぉ（x）", "ぁぃぅぇぉ"),
        //new TypingText("ゃゅょ（l）", "ゃゅょ"),
        //new TypingText("ゃゅょ（x）", "ゃゅょ"),

        new TypingText("遊んでいただきありがとうございます", "あそんでいただきありがとうございます"),
        new TypingText("このゲームは未完成です", "このげーむはみかんせいです"),
        new TypingText("お楽しみに", "おたのしみに"),


        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("ぎゃぎゅぎょ", "ぎゃぎゅぎょ"),
        //new TypingText("ぎゃぎゅぎょ(lya, lyu, lyo)", "ぎゃぎゅぎょ"),
        //new TypingText("ぎゃぎゅぎょ(xya, xyu, xyo)", "ぎゃぎゅぎょ"),
        //new TypingText("しゃしゅしょ", "しゃしゅしょ"),
        //new TypingText("しゃしゅしょ(lya, lyu, lyo)", "しゃしゅしょ"),
        //new TypingText("しゃしゅしょ(xya, xyu, xyo)", "しゃしゅしょ"),
        //new TypingText("じゃじゅじょ", "じゃじゅじょ"),
        //new TypingText("じゃじゅじょ(lya, lyu, lyo)", "じゃじゅじょ"),
        //new TypingText("じゃじゅじょ(xya, xyu, xyo)", "じゃじゅじょ"),
        //new TypingText("ちゃちゅちょ", "ちゃちゅちょ"),
        //new TypingText("ちゃちゅちょ(lya, lyu, lyo)", "ちゃちゅちょ"),
        //new TypingText("ちゃちゅちょ(xya, xyu, xyo)", "ちゃちゅちょ"),
        //new TypingText("ぢ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(lya, lyu, lyo)", "きゃきゅきょ"),
        //new TypingText("きゃきゅきょ(xya, xyu, xyo)", "きゃきゅきょ"),

        //new TypingText("あいうえお", "あいうえお"),


        //new TypingText("絶対に浪人できなくなってしまった", "ぜったいにろうにんできなくなってしまった"),
        //new TypingText("studyplusで名前がイタリック体の人は女子がち", "studyplusでなまえがいたりっくたいのひとはじょしがち"),
        //new TypingText("東大一次合格しました", "とうだいいちじごうかくしました"),
        //new TypingText("東大虹合格しました", "とうだいにじごうかくしました"),
        //new TypingText("引っ越しの時、ミニマリストは輝く", "ひっこしのとき、みにまりすとはかがやく"),
        //new TypingText("モノ売るってレベルじゃねぇぞ！！", "ものうるってれべるじゃねえぞ！！"),
        //new TypingText("ホームページ作ったにょおおおおおおおおおおん！！", "ほーむぺーじつくったにょおおおおおおおおおおん！！"),
        //new TypingText("ばおわ", "ばおわ"),
        //new TypingText("全然忘れてたけど、一応うちの学校バイト禁止なのか", "ぜんぜんわすれてたけど、いちおううちのがっこうばいときんしなのか"),
        //new TypingText("チェンソーマン1日で11話まで見てしまった", "ちぇんそーまん1にちで11わまでみてしまった"),
    };

    void Start()
    {
        audioSource = mainCamera.GetComponent<AudioSource>();
        missRedImage = missRedPanel.GetComponent<Image>();

        isClear = false;
        nowHiraganaIndex = 0;
        nowAlphabetIndex = 0;
        nowTypingLetterIndex = 0;
        nowTextIndex = 0;
        alphabetStrings = new List<string> { };

        keyPushFlag = false;

        //TypingTexts = GetRandomElements(TextDataBase, 1);
        TypingTexts = TextDataBase;

        displayText = japaneseTextToAlphabetText(TypingTexts[0].japaneseText);
        alphabetText.text = displayText;
    }

    void FixedUpdate()
    {
        missRedImage.color = Color.Lerp(missRedImage.color, Color.clear, Time.deltaTime*2.0f);

        if (isClear == false) {
            nowTypingText = TypingTexts[nowTextIndex];
            if (nowHiraganaIndex == 0 && nowAlphabetIndex == 0)
            {
                displayText = japaneseTextToAlphabetText(nowTypingText.hiraganaText);
            }
            japaneseText.text = nowTypingText.japaneseText;
            hiraganaText.text = nowTypingText.hiraganaText;
            alphabetText.text = "<color=red>" + displayText.Substring(0, nowTypingLetterIndex) + "</color>" + displayText.Substring(nowTypingLetterIndex);
            nowHiraganaLetter = nowTypingText.hiraganaText[nowHiraganaIndex];

            if (firstAlphabetCheckList.Contains(nowHiraganaLetter) && nowAlphabetIndex == 0)
            {
                if (nowHiraganaLetter == 'く') {
                    alphabetStrings.Add("ku");
                    alphabetStrings.Add("cu");
                    alphabetStrings.Add("qu");

                    replaceAlphabetIndex1 = nowTypingLetterIndex;
                    replaceAlphabetIndex2 = nowTypingLetterIndex + 2;

                    if (Input.GetKey(KeyCode.K))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 0;
                            Debug.Log(alphabetStrings[selectAlphabetIndex]);
                            Debug.Log(nowAlphabetIndex);
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                        }
                    }
                    else if (Input.GetKey(KeyCode.C))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 1;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, replaceAlphabetIndex1) + "cu" + displayText.Substring(replaceAlphabetIndex2);
                        }
                    }
                    else if (Input.GetKey(KeyCode.Q))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 2;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, replaceAlphabetIndex1) + "qu" + displayText.Substring(replaceAlphabetIndex2);
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                }
                else {
                    switch (nowHiraganaLetter)
                    {
                        case 'か':
                            alphabetStrings.Add("ka");
                            alphabetStrings.Add("ca");
                            keycode1 = KeyCode.K;
                            keycode2 = KeyCode.C;
                            replaceAlphabet = "ca";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'こ':
                            alphabetStrings.Add("ko");
                            alphabetStrings.Add("co");
                            keycode1 = KeyCode.K;
                            keycode2 = KeyCode.C;
                            replaceAlphabet = "co";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'せ':
                            alphabetStrings.Add("se");
                            alphabetStrings.Add("ce");
                            keycode1 = KeyCode.S;
                            keycode2 = KeyCode.C;
                            replaceAlphabet = "ce";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ち':
                            alphabetStrings.Add("ti");
                            alphabetStrings.Add("chi");
                            keycode1 = KeyCode.T;
                            keycode2 = KeyCode.C;
                            replaceAlphabet = "chi";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ふ':
                            alphabetStrings.Add("hu");
                            alphabetStrings.Add("fu");
                            keycode1 = KeyCode.H;
                            keycode2 = KeyCode.F;
                            replaceAlphabet = "fu";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'じ':
                            alphabetStrings.Add("zi");
                            alphabetStrings.Add("ji");
                            keycode1 = KeyCode.Z;
                            keycode2 = KeyCode.J;
                            replaceAlphabet = "ji";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ぁ':
                            alphabetStrings.Add("la");
                            alphabetStrings.Add("xa");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xa";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ぃ':
                            alphabetStrings.Add("li");
                            alphabetStrings.Add("xi");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xi";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ぅ':
                            alphabetStrings.Add("lu");
                            alphabetStrings.Add("xu");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xu";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ぇ':
                            alphabetStrings.Add("le");
                            alphabetStrings.Add("xe");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xe";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ぉ':
                            alphabetStrings.Add("lo");
                            alphabetStrings.Add("xo");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xo";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 2;
                            break;
                        case 'ゃ':
                            alphabetStrings.Add("lya");
                            alphabetStrings.Add("xya");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xya";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 3;
                            break;
                        case 'ゅ':
                            alphabetStrings.Add("lyu");
                            alphabetStrings.Add("xyu");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xyu";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 3;
                            break;
                        case 'ょ':
                            alphabetStrings.Add("lyo");
                            alphabetStrings.Add("xyo");
                            keycode1 = KeyCode.L;
                            keycode2 = KeyCode.X;
                            replaceAlphabet = "xyo";
                            replaceAlphabetIndex1 = nowTypingLetterIndex;
                            replaceAlphabetIndex2 = nowTypingLetterIndex + 3;
                            break;

                    }

                    if (Input.GetKey(keycode1))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 0;
                            Debug.Log(alphabetStrings[selectAlphabetIndex]);
                            Debug.Log(nowAlphabetIndex);
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                        }
                    }
                    else if (Input.GetKey(keycode2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 1;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, replaceAlphabetIndex1) + replaceAlphabet + displayText.Substring(replaceAlphabetIndex2);
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                }
            }
            else if (nowHiraganaLetter == 'ん' && nowAlphabetIndex == 0)
            {
                alphabetStrings.Add("nn");
                alphabetStrings.Add("xn");
                if (Input.GetKey(KeyCode.N))
                {
                    if (keyPushFlag == false)
                    {
                        keyPushFlag = true;
                        Debug.Log("あ");
                        selectAlphabetIndex = 0;
                        alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                        nowAlphabetIndex += 1;
                        nowTypingLetterIndex += 1;
                        playTypingSound();
                    }
                }
                else if (Input.GetKey(KeyCode.X))
                {
                    if (keyPushFlag == false)
                    {
                        keyPushFlag = true;
                        selectAlphabetIndex = 1;
                        alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                        nowAlphabetIndex += 1;
                        nowTypingLetterIndex += 1;
                        playTypingSound();

                        if (nowHiraganaIndex != nowTypingText.hiraganaText.Length - 1 && !nNextHiraganaCheckList.Contains(nowTypingText.hiraganaText[nowHiraganaIndex + 1]))
                        {
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "xn" + displayText.Substring(nowTypingLetterIndex);
                        } else
                        {
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "x" + displayText.Substring(nowTypingLetterIndex);
                        }   
                    }
                }
                else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                {
                    if (keyPushFlag == false)
                    {
                        keyPushFlag = true;
                        RandomizeSfx(missSound);
                        missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                    }
                }
                else
                {
                    keyPushFlag = false;
                }
            }
            else if (nowHiraganaLetter == 'ん' && nowAlphabetIndex == 1 && selectAlphabetIndex == 0) // ん（nn)の1文字目を打った段階のとき
            {
                //「ん」が文末になく、次の文字が母音、な行、にゃ行、「ん」でもない場合
                if (nowHiraganaIndex != nowTypingText.hiraganaText.Length - 1 && !nNextHiraganaCheckList.Contains(nowTypingText.hiraganaText[nowHiraganaIndex + 1]))
                {
                    if (Input.GetKey(KeyCode.N))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "n" + displayText.Substring(nowTypingLetterIndex - 1);
                            if (nowAlphabetIndex >= alphabetStrings[selectAlphabetIndex].Length)
                            {
                                nowHiraganaIndex += 1;
                                Debug.Log("あ");
                                nowAlphabetIndex = 0;
                                alphabetStrings.Clear();
                            }
                        }
                    }
                    else if (Input.GetKey(letterToKeyCode(hiraganaToAlphabet(nowTypingText.hiraganaText[nowHiraganaIndex + 1])[0])))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            alphabetStrings.Clear();
                            nowHiraganaIndex += 1;
                            nowHiraganaLetter = nowTypingText.hiraganaText[nowHiraganaIndex];

                            alphabetStrings.Add(hiraganaToAlphabet(nowHiraganaLetter));
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;

                            nowAlphabetIndex = 1;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];

                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            if (nowAlphabetIndex >= alphabetStrings[selectAlphabetIndex].Length)
                            {
                                nowHiraganaIndex += 1;
                                Debug.Log("あ");
                                nowAlphabetIndex = 0;
                                alphabetStrings.Clear();
                            }
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                } else
                {
                    if (Input.GetKey(KeyCode.N))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            if (nowAlphabetIndex >= alphabetStrings[selectAlphabetIndex].Length)
                            {
                                nowHiraganaIndex += 1;
                                nowAlphabetIndex = 0;
                                alphabetStrings.Clear();
                            }
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                }
            }
            else if (nowHiraganaLetter == 'っ' && nowAlphabetIndex == 0)
            {
                //「っ」が文末になく、次の文字が母音、「っ」でもない場合
                if (nowHiraganaIndex != nowTypingText.hiraganaText.Length - 1 && !tuNextHiraganaCheckList.Contains(nowTypingText.hiraganaText[nowHiraganaIndex + 1]))
                {
                    alphabetStrings.Add("ltu");
                    alphabetStrings.Add("xtu");
                    alphabetStrings.Add(hiraganaToAlphabet(nowTypingText.hiraganaText[nowHiraganaIndex + 1]));
                    if (Input.GetKey(KeyCode.L))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "ltu" + displayText.Substring(nowTypingLetterIndex);
                        }
                    }
                    else if (Input.GetKey(KeyCode.X))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "xtu" + displayText.Substring(nowTypingLetterIndex);
                        }
                    }
                    else if (Input.GetKey(letterToKeyCode(alphabetStrings[2][0])))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            selectAlphabetIndex = 2;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            nowHiraganaIndex += 1;
                            nowAlphabetIndex = 0;
                            alphabetStrings.Clear();
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                }
                else //xtuかltuの打ち方しかできない場合
                {
                    alphabetStrings.Add("ltu");
                    alphabetStrings.Add("xtu");
                    if (Input.GetKey(KeyCode.L))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "ltu" + displayText.Substring(nowTypingLetterIndex + 2);
                        }
                    }
                    else if (Input.GetKey(KeyCode.X))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            Debug.Log("あ");
                            selectAlphabetIndex = 0;
                            alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                            nowAlphabetIndex += 1;
                            nowTypingLetterIndex += 1;
                            playTypingSound();
                            displayText = displayText.Substring(0, nowTypingLetterIndex - 1) + "xtu" + displayText.Substring(nowTypingLetterIndex + 2);
                        }
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                    {
                        if (keyPushFlag == false)
                        {
                            keyPushFlag = true;
                            RandomizeSfx(missSound);
                            missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                        }
                    }
                    else
                    {
                        keyPushFlag = false;
                    }
                }
            }
            else //その他の文字
            {
                if ((nowHiraganaLetter == 'ん' && selectAlphabetIndex == 1) || nowHiraganaLetter == 'ち') {
                    //pass
                }
                else
                {
                    alphabetStrings.Add(hiraganaToAlphabet(nowHiraganaLetter));
                    selectAlphabetIndex = 0;
                }
                alphabetLetter = alphabetStrings[selectAlphabetIndex][nowAlphabetIndex];
                if (Input.GetKey(letterToKeyCode(alphabetLetter)))
                {
                    if (keyPushFlag == false)
                    {
                        keyPushFlag = true;
                        nowAlphabetIndex += 1;
                        nowTypingLetterIndex += 1;
                        playTypingSound();
                        if (nowAlphabetIndex >= alphabetStrings[selectAlphabetIndex].Length)
                        {
                            nowHiraganaIndex += 1;
                            nowAlphabetIndex = 0;
                            alphabetStrings.Clear();
                        }
                    }
                }
                else if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                {
                    if (keyPushFlag == false)
                    {
                        keyPushFlag = true;
                        RandomizeSfx(missSound);
                        missRedImage.color = new Color(1.0f, 0, 0, 0.7f);
                    }
                } else
                {
                    keyPushFlag = false;
                }
            }
            if (nowHiraganaIndex >= nowTypingText.hiraganaText.Length)
            {
                nowHiraganaIndex = 0;
                nowAlphabetIndex = 0;
                nowTypingLetterIndex = 0;
                nowTextIndex += 1;
            }
            if (nowTextIndex >= TypingTexts.Count)
            {
                isClear = true;
            }
        } else
        {
            japaneseText.gameObject.SetActive(false);
            hiraganaText.gameObject.SetActive(false);
            alphabetText.gameObject.SetActive(false);
            clearText.gameObject.SetActive(true);
        }
        
    }

    string japaneseTextToAlphabetText(string japaneseText)
    {
        string alphabetText = "";
        int i = 0;
        while (i < japaneseText.Length)
        {
            char hiraganaText = japaneseText[i];
            if (hiraganaText == 'っ' && i < japaneseText.Length - 1 && !tuNextHiraganaCheckList.Contains(japaneseText[i + 1]))
            {
                i += 1;
                hiraganaText = japaneseText[i];
                alphabetText += hiraganaToAlphabet(hiraganaText)[0] + hiraganaToAlphabet(hiraganaText);
            }
            else if (hiraganaText == 'ん' && i != japaneseText.Length - 1 && !nNextHiraganaCheckList.Contains(japaneseText[i + 1]))
            {
                hiraganaText = japaneseText[i];
                alphabetText += hiraganaToAlphabet(hiraganaText)[0];
            }
            else
            {
                alphabetText += hiraganaToAlphabet(hiraganaText);
            }
            i += 1;
        }
        return alphabetText;
    }

    private List<TypingText> GetRandomElements(List<TypingText> list, int count)
    {
        List<TypingText> result = new List<TypingText>();
        System.Random rand = new System.Random();

        // リストからランダムに要素を選択
        for (int i = 0; i < count; i++)
        {
            int randomIndex = rand.Next(list.Count);
            result.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }

        return result;
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        var randomIndex = UnityEngine.Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[randomIndex]);
    }

    void playTypingSound()
    {
        switch (typingSound)
        {
            case 0:
                RandomizeSfx(typingSoundA1, typingSoundA2, typingSoundA3, typingSoundA4, typingSoundA5, typingSoundA6);
                break;
            case 1:
                RandomizeSfx(typingSoundB1, typingSoundB2, typingSoundB3);
                break;
            case 2:
                RandomizeSfx(typingSoundC1, typingSoundC2, typingSoundC3);
                break;
            case 3:
                RandomizeSfx(typingSoundD1, typingSoundD2, typingSoundD3);
                break;
            case 4:
                RandomizeSfx(typingSoundE1, typingSoundE2, typingSoundE3);
                break;
            case 5:
                RandomizeSfx(typingSoundF1, typingSoundF2, typingSoundF3);
                break;
            case 6:
                RandomizeSfx(typingSoundG);
                break;
        }
    }

    string hiraganaToAlphabet(char targetHiragana)
    {
        switch (targetHiragana)
        {
            case 'a':
                return "a";
            case 'b':
                return "b";
            case 'c':
                return "c";
            case 'd':
                return "d";
            case 'e':
                return "e";
            case 'f':
                return "f";
            case 'g':
                return "g";
            case 'h':
                return "h";
            case 'i':
                return "i";
            case 'j':
                return "j";
            case 'k':
                return "k";
            case 'l':
                return "l";
            case 'm':
                return "m";
            case 'n':
                return "n";
            case 'o':
                return "o";
            case 'p':
                return "p";
            case 'q':
                return "q";
            case 'r':
                return "r";
            case 's':
                return "s";
            case 't':
                return "t";
            case 'u':
                return "u";
            case 'v':
                return "v";
            case 'w':
                return "w";
            case 'x':
                return "x";
            case 'y':
                return "y";
            case 'z':
                return "z";
            case 'あ':
                return "a";
            case 'い':
                return "i";
            case 'う':
                return "u";
            case 'え':
                return "e";
            case 'お':
                return "o";
            case 'か':
                return "ka";
            case 'き':
                return "ki";
            case 'く':
                return "ku";
            case 'け':
                return "ke";
            case 'こ':
                return "ko";
            case 'さ':
                return "sa";
            case 'し':
                return "si";
            case 'す':
                return "su";
            case 'せ':
                return "se";
            case 'そ':
                return "so";
            case 'た':
                return "ta";
            case 'ち':
                return "ti";
            case 'つ':
                return "tu";
            case 'て':
                return "te";
            case 'と':
                return "to";
            case 'な':
                return "na";
            case 'に':
                return "ni";
            case 'ぬ':
                return "nu";
            case 'ね':
                return "ne";
            case 'の':
                return "no";
            case 'は':
                return "ha";
            case 'ひ':
                return "hi";
            case 'ふ':
                return "hu";
            case 'へ':
                return "he";
            case 'ほ':
                return "ho";
            case 'ま':
                return "ma";
            case 'み':
                return "mi";
            case 'む':
                return "mu";
            case 'め':
                return "me";
            case 'も':
                return "mo";
            case 'や':
                return "ya";
            case 'ゆ':
                return "yu";
            case 'よ':
                return "yo";
            case 'ら':
                return "ra";
            case 'り':
                return "ri";
            case 'る':
                return "ru";
            case 'れ':
                return "re";
            case 'ろ':
                return "ro";
            case 'わ':
                return "wa";
            case 'を':
                return "wo";
            case 'ん':
                return "nn";
            case 'ぁ':
                return "la";
            case 'ぃ':
                return "li";
            case 'ぅ':
                return "lu";
            case 'ぇ':
                return "le";
            case 'ぉ':
                return "lo";
            case 'ゃ':
                return "lya";
            case 'ゅ':
                return "lyu";
            case 'ょ':
                return "lyo";
            case 'が':
                return "ga";
            case 'ぎ':
                return "gi";
            case 'ぐ':
                return "gu";
            case 'げ':
                return "ge";
            case 'ご':
                return "go";
            case 'ざ':
                return "za";
            case 'じ':
                return "zi";
            case 'ず':
                return "zu";
            case 'ぜ':
                return "ze";
            case 'ぞ':
                return "zo";
            case 'だ':
                return "da";
            case 'ぢ':
                return "di";
            case 'づ':
                return "du";
            case 'で':
                return "de";
            case 'ど':
                return "do";
            case 'ば':
                return "ba";
            case 'び':
                return "bi";
            case 'ぶ':
                return "bu";
            case 'べ':
                return "be";
            case 'ぼ':
                return "bo";
            case 'ぱ':
                return "pa";
            case 'ぴ':
                return "pi";
            case 'ぷ':
                return "pu";
            case 'ぺ':
                return "pe";
            case 'ぽ':
                return "po";
            case 'っ':
                return "ltu";
            case 'ー':
                return "-";
            case '、':
                return ",";
            default:
                return "";
        }
    }

    KeyCode letterToKeyCode(char targetChar)
    {
        switch (targetChar)
        {
            case 'a':
                return KeyCode.A;
            case 'b':
                return KeyCode.B;
            case 'c':
                return KeyCode.C;
            case 'd':
                return KeyCode.D;
            case 'e':
                return KeyCode.E;
            case 'f':
                return KeyCode.F;
            case 'g':
                return KeyCode.G;
            case 'h':
                return KeyCode.H;
            case 'i':
                return KeyCode.I;
            case 'j':
                return KeyCode.J;
            case 'k':
                return KeyCode.K;
            case 'l':
                return KeyCode.L;
            case 'm':
                return KeyCode.M;
            case 'n':
                return KeyCode.N;
            case 'o':
                return KeyCode.O;
            case 'p':
                return KeyCode.P;
            case 'q':
                return KeyCode.Q;
            case 'r':
                return KeyCode.R;
            case 's':
                return KeyCode.S;
            case 't':
                return KeyCode.T;
            case 'u':
                return KeyCode.U;
            case 'v':
                return KeyCode.V;
            case 'w':
                return KeyCode.W;
            case 'x':
                return KeyCode.X;
            case 'y':
                return KeyCode.Y;
            case 'z':
                return KeyCode.Z;
            case '-':
                return KeyCode.Minus;
            case ',':
                return KeyCode.Comma;
            default:
                return KeyCode.None;
        }
    }
}