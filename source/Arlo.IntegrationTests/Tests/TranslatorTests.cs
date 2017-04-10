using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBot.SupportLibrary.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class TranslatorTests : TestBase
    {
        [TestMethod]
        public async Task TestDetectLanguage()
        {
            var translator = Resolve<ITranslatorService>();

            var chinese = "明天会发生什么";
            var japanese = "明日の予定";
            var english = "what's on tomorrow";
            var italian = "ciò che è domani";
            var klingon = "jIHvaD Suq naDev";

            var resultChinese = await translator.Detect(chinese);
            var resultjapanese = await translator.Detect(japanese);
            var resultenglish = await translator.Detect(english);
            var resultitalian = await translator.Detect(italian);
            var resultKlingong = await translator.Detect(klingon);

            Assert.AreEqual("en", resultenglish);
            Assert.AreEqual("ja", resultjapanese);
            Assert.AreEqual("zh-CHS", resultChinese);
            Assert.AreEqual("it", resultitalian);
        }

        [TestMethod]
        public async Task TranslateLanguage()
        {
            var translator = Resolve<ITranslatorService>();

            var chinese = "明天会发生什么";
            var japanese = @"受 取 番 号 
お 受 け 取 り 窓 口 6 番 に
下 記 の 番 号 が 表 示 さ れ ま し た
ら こ の 札 と 引 換 え に 、 申 請 し
た 書 類 を お 受 け 取 り く だ さ い
2 2 0
住 民 票 ・ 年 金 の 現 況 届 
、 戸 籍 謄 本 ・ 抄 本 、 附 票 ・ 身 分 証 明
ト 印 鑑 登 録 証 明 書 ・ 課 税 、 納 税 証 明 書 等
中 野 区 役 所 証 明 担 当 ";
            var english = "what's on tomorrow";
            var italian = "ciò che è domani";


            var resultChinese = await translator.Detect(chinese);
            var resultjapanese = await translator.Detect(japanese);
            var resultenglish = await translator.Detect(english);
            var resultitalian = await translator.Detect(italian);

            var translatedChinese = await translator.Translate(chinese, resultChinese);
            var translatedjapanese = await translator.Translate(japanese, resultjapanese);

            var translatedenglish = await translator.Translate(english, resultenglish);
            var translateditalian = await translator.Translate(italian, resultitalian);


            

        }
    }
}
