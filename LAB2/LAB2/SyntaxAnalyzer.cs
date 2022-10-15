using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    // Класс исключительных ситуаций синтаксического анализа.
    class SynAnException : Exception
    {
        // Позиция возникновения исключительной ситуации в анализируемом тексте.
        private int lineIndex; // Индекс строки.
        private int symIndex;  // Индекс символа.

        // Индекс строки, где возникла исключительная ситуация - свойство только для чтения.
        public int LineIndex
        {
            get { return lineIndex; }
        }

        // Индекс символа, на котором возникла исключительная ситуация - свойство только для чтения.
        public int SymIndex
        {
            get { return symIndex; }
        }

        // Конструктор исключительной ситуации.
        // message - описание исключительной ситуации.
        // lineIndex и symIndex - позиция возникновения исключительной ситуации в анализируемом тексте.
        public SynAnException(string message, int lineIndex, int symIndex)
            : base(message)
        {
            this.lineIndex = lineIndex;
            this.symIndex = symIndex;
        }
    }

    // Класс "Синтаксический анализатор".
    // При обнаружении ошибки в исходном тексте он генерирует исключительную ситуацию SynAnException или LexAnException.
    class SyntaxAnalyzer
    {
        private LexicalAnalyzer lexAn; // Лексический анализатор.

        // Конструктор синтаксического анализатора. 
        // В качестве параметра передается исходный текст.
        public SyntaxAnalyzer(string[] inputLines)
        {
            // Создаем лексический анализатор.
            // Передаем ему текст.
            lexAn = new LexicalAnalyzer(inputLines);
        }

        // Обработать синтаксическую ошибку.
        // msg - описание ошибки.
        private void SyntaxError(string msg)
        {
            // Генерируем исключительную ситуацию, тем самым полностью прерывая процесс анализа текста.
            throw new SynAnException(msg, lexAn.CurLineIndex, lexAn.CurSymIndex);
        }

        // Проверить, что тип текущего распознанного токена совпадает с заданным.
        // Если совпадает, то распознать следующий токен, иначе синтаксическая ошибка.
        private void Match(TokenKind tkn)
        {
            if (lexAn.Token.Type == tkn) // Сравниваем.
            {
                lexAn.RecognizeNextToken(); // Распознаем следующий токен.
            }
            else 
            {
                SyntaxError("Ожидалось " + tkn.ToString()); // Обнаружена синтаксическая ошибка.
            }
        }


        // Провести синтаксический анализ текста.
        public void ParseText()
        {
            P();

            if (lexAn.Token.Type != TokenKind.EndOfText) // Если текущий токен не является концом текста.
            {
                SyntaxError("После арифметического выражения идет еще какой-то текст"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void P()
        {
            lexAn.RecognizeNextToken(); // Распознаем первый токен в тексте.
            K();
            if (lexAn.Token.Type != TokenKind.EndOfText) // Если текущий токен не является концом текста.
            {
                S();
            }
        }


        private void K()
        {
            O();
            R();
            if(lexAn.Token.Type == TokenKind.Comma)
            {
                lexAn.RecognizeNextToken();
            }
            else
            {
                SyntaxError("Ожидалась запятая");
            }
            A();
        }

        private void S()
        {
            if(lexAn.Token.Type == TokenKind.Semicolon)
            {
                P();
            }
            else
            {
                SyntaxError("После арифметического выражения идет еще какой-то текст"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void O()
        {
            if (lexAn.Token.Type == TokenKind.Identifier) // Если текущий токен - идентификатор.
            {
                lexAn.RecognizeNextToken(); // Пропускаем этот идентификатор.
            }
            else
            {
                SyntaxError("Ожидались буквы"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void R()
        {
            if (lexAn.Token.Type == TokenKind.Number) // Если текущий токен - число.
            {
                
                lexAn.RecognizeNextToken(); // Пропускаем этот идентификатор.
            }
            else
            {
                SyntaxError("Ожидались цифры"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void A()
        {
            if (lexAn.Token.Type == TokenKind.Number)
            {
                lexAn.RecognizeNextToken();
            }
            else if (lexAn.Token.Type == TokenKind.Identifier)
            {
                lexAn.RecognizeNextToken();
            }
            else
            {
                SyntaxError("Ожидались буква (a,b,c,d) или цифра (1,0)"); // Обнаружена синтаксическая ошибка.
            }
        }

    }
}
