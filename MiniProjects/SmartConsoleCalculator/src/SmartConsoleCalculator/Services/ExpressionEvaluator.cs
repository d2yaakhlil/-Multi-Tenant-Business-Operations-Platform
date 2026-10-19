using System.Globalization;

namespace SmartConsoleCalculator.Services;

/// <summary>
/// Specifies the type of lexical token in a mathematical expression.
/// </summary>
internal enum TokenType
{
    Number,
    Plus,
    Minus,
    Multiply,
    Divide,
    Modulus,
    LeftParen,
    RightParen
}

/// <summary>
/// Represents an immutable token produced by the tokenizer.
/// </summary>
internal readonly struct Token : IEquatable<Token>
{
    public TokenType Type { get; }
    public double Value { get; }
    public string Text { get; }
    public int Position { get; }
    public bool IsUnary { get; }

    public Token(TokenType type, string text, int position, double value = 0.0, bool isUnary = false)
    {
        Type = type;
        Text = text;
        Position = position;
        Value = value;
        IsUnary = isUnary;
    }

    public bool Equals(Token other) =>
        Type == other.Type &&
        Value.Equals(other.Value) &&
        Text == other.Text &&
        Position == other.Position &&
        IsUnary == other.IsUnary;

    public override bool Equals(object? obj) => obj is Token other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Type, Value, Text, Position, IsUnary);

    public static bool operator ==(Token left, Token right) => left.Equals(right);

    public static bool operator !=(Token left, Token right) => !left.Equals(right);

    public override string ToString() => Type switch
    {
        TokenType.Number => $"Number({Value.ToString(CultureInfo.InvariantCulture)})",
        _ => IsUnary ? $"Unary{Type}('{Text}')" : $"{Type}('{Text}')"
    };
}

/// <summary>
/// Represents an error encountered during expression tokenization or parsing.
/// </summary>
internal class ExpressionParseException : Exception
{
    public int Position { get; }

    public ExpressionParseException(string message, int position = -1) : base(message)
    {
        Position = position;
    }
}

/// <summary>
/// Evaluates mathematical expressions using a recursive-descent parser.
/// Supports standard precedence, associativity, parentheses, decimal numbers, and unary operators.
/// </summary>
public class ExpressionEvaluator
{
    private readonly object _syncLock = new();
    private List<Token> _tokens = new();
    private int _position;

    private Token Current => _position < _tokens.Count ? _tokens[_position] : default;
    private bool IsAtEnd => _position >= _tokens.Count;

    /// <summary>
    /// Evaluates a mathematical expression string and returns the result or an error message.
    /// </summary>
    /// <param name="expression">The mathematical expression to evaluate.</param>
    /// <returns>A tuple indicating success, the numeric result, and an error message if failed.</returns>
    public (bool Success, double Result, string Error) Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return (false, 0.0, "Expression cannot be empty.");
        }

        expression = expression.Trim().Trim('\uFEFF');

        lock (_syncLock)
        {
            try
            {
                _tokens = Tokenize(expression);
                if (_tokens.Count == 0)
                {
                    return (false, 0.0, "Expression cannot be empty.");
                }

                _position = 0;
                double result = ParseExpression();

                if (!IsAtEnd)
                {
                    if (Current.Type == TokenType.RightParen)
                    {
                        return (false, 0.0, $"Mismatched parentheses. Unexpected closing ')' at position {Current.Position}.");
                    }

                    return (false, 0.0, $"Unexpected token '{Current.Text}' at position {Current.Position}.");
                }

                if (!double.IsFinite(result))
                {
                    return (false, 0.0, "Expression result is not a finite number.");
                }

                return (true, result, string.Empty);
            }
            catch (ExpressionParseException ex)
            {
                return (false, 0.0, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, 0.0, $"Evaluation error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Tokenizes the mathematical expression into a list of strongly typed tokens.
    /// Rejects unsupported characters and detects invalid implicit multiplication syntax.
    /// </summary>
    /// <param name="expression">The expression string to tokenize.</param>
    /// <returns>A list of recognized tokens.</returns>
    /// <exception cref="ExpressionParseException">Thrown when lexical syntax or character is invalid.</exception>
    internal List<Token> Tokenize(string expression)
    {
        var tokens = new List<Token>();
        int index = 0;

        while (index < expression.Length)
        {
            char c = expression[index];

            if (char.IsWhiteSpace(c))
            {
                index++;
                continue;
            }

            Token? lastToken = tokens.Count > 0 ? tokens[^1] : null;

            // Numbers: either starts with digit or decimal point followed by digit
            if (char.IsDigit(c) || (c == '.' && index + 1 < expression.Length && char.IsDigit(expression[index + 1])))
            {
                // Disallow implicit multiplication: Number followed directly by Number or ')' followed by Number
                if (lastToken.HasValue)
                {
                    if (lastToken.Value.Type == TokenType.Number)
                    {
                        throw new ExpressionParseException($"Unexpected number at position {index}. Missing operator between numbers.", index);
                    }
                    if (lastToken.Value.Type == TokenType.RightParen)
                    {
                        throw new ExpressionParseException($"Implicit multiplication is not supported after closing parenthesis at position {index}. Use '*' explicitly.", index);
                    }
                }

                int startPos = index;
                bool hasDecimal = false;

                while (index < expression.Length)
                {
                    char current = expression[index];
                    if (char.IsDigit(current))
                    {
                        index++;
                    }
                    else if (current == '.')
                    {
                        if (hasDecimal)
                        {
                            throw new ExpressionParseException($"Invalid number format with multiple decimal points at position {index}.", index);
                        }
                        hasDecimal = true;
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }

                string numText = expression.Substring(startPos, index - startPos);
                if (!double.TryParse(numText, NumberStyles.Float, CultureInfo.InvariantCulture, out double numVal) || !double.IsFinite(numVal))
                {
                    throw new ExpressionParseException($"Invalid numeric value '{numText}' at position {startPos}.", startPos);
                }

                tokens.Add(new Token(TokenType.Number, numText, startPos, numVal));
                continue;
            }

            // Left parenthesis
            if (c == '(')
            {
                // Disallow implicit multiplication: Number followed by '(' or ')' followed by '('
                if (lastToken.HasValue)
                {
                    if (lastToken.Value.Type == TokenType.Number)
                    {
                        throw new ExpressionParseException($"Implicit multiplication is not supported before '(' at position {index}. Use '*' explicitly.", index);
                    }
                    if (lastToken.Value.Type == TokenType.RightParen)
                    {
                        throw new ExpressionParseException($"Implicit multiplication is not supported between parentheses at position {index}. Use '*' explicitly.", index);
                    }
                }

                tokens.Add(new Token(TokenType.LeftParen, "(", index));
                index++;
                continue;
            }

            // Right parenthesis
            if (c == ')')
            {
                tokens.Add(new Token(TokenType.RightParen, ")", index));
                index++;
                continue;
            }

            // Operators: +, -
            if (c == '+' || c == '-')
            {
                // Unary when at the start of expression, after an open parenthesis, or after another operator
                bool isUnary = !lastToken.HasValue ||
                               lastToken.Value.Type == TokenType.LeftParen ||
                               IsOperator(lastToken.Value.Type);

                TokenType opType = c == '+' ? TokenType.Plus : TokenType.Minus;
                tokens.Add(new Token(opType, c.ToString(), index, isUnary: isUnary));
                index++;
                continue;
            }

            // Operator: *
            if (c == '*')
            {
                tokens.Add(new Token(TokenType.Multiply, "*", index));
                index++;
                continue;
            }

            // Operator: /
            if (c == '/')
            {
                tokens.Add(new Token(TokenType.Divide, "/", index));
                index++;
                continue;
            }

            // Operator: %
            if (c == '%')
            {
                tokens.Add(new Token(TokenType.Modulus, "%", index));
                index++;
                continue;
            }

            // Any unsupported character (including letters, isolated '.', or unknown symbols)
            throw new ExpressionParseException($"Unsupported character '{c}' at position {index}.", index);
        }

        return tokens;
    }

    /// <summary>
    /// Parses addition and subtraction expressions (lowest binary precedence).
    /// </summary>
    internal double ParseExpression()
    {
        double left = ParseTerm();

        while (!IsAtEnd)
        {
            if (Current.Type == TokenType.Plus && !Current.IsUnary)
            {
                _position++;
                double right = ParseTerm();
                double result = left + right;
                if (!double.IsFinite(result))
                {
                    throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.");
                }
                left = result;
            }
            else if (Current.Type == TokenType.Minus && !Current.IsUnary)
            {
                _position++;
                double right = ParseTerm();
                double result = left - right;
                if (!double.IsFinite(result))
                {
                    throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.");
                }
                left = result;
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Parses multiplication, division, and modulus operations (higher binary precedence).
    /// </summary>
    internal double ParseTerm()
    {
        double left = ParseFactor();

        while (!IsAtEnd)
        {
            if (Current.Type == TokenType.Multiply)
            {
                Token op = Current;
                _position++;
                double right = ParseFactor();
                double result = left * right;
                if (!double.IsFinite(result))
                {
                    throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.", op.Position);
                }
                left = result;
            }
            else if (Current.Type == TokenType.Divide)
            {
                Token op = Current;
                _position++;
                double right = ParseFactor();
                if (right == 0.0)
                {
                    throw new ExpressionParseException("Division by zero is not allowed.", op.Position);
                }
                double result = left / right;
                if (!double.IsFinite(result))
                {
                    throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.", op.Position);
                }
                left = result;
            }
            else if (Current.Type == TokenType.Modulus)
            {
                Token op = Current;
                _position++;
                double right = ParseFactor();
                if (right == 0.0)
                {
                    throw new ExpressionParseException("Modulus by zero is not allowed.", op.Position);
                }
                double result = left % right;
                if (!double.IsFinite(result))
                {
                    throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.", op.Position);
                }
                left = result;
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Parses factors: unary operations (+, -), parenthesized sub-expressions, and numbers.
    /// </summary>
    internal double ParseFactor()
    {
        if (IsAtEnd)
        {
            Token prev = _position > 0 ? _tokens[_position - 1] : default;
            string detail = prev.Type != default ? $" after '{prev.Text}' at position {prev.Position}" : string.Empty;
            throw new ExpressionParseException($"Unexpected end of expression. Expected an operand{detail}.");
        }

        // Unary plus
        if (Current.Type == TokenType.Plus && Current.IsUnary)
        {
            _position++;
            return ParseFactor();
        }

        // Unary minus
        if (Current.Type == TokenType.Minus && Current.IsUnary)
        {
            _position++;
            double factor = ParseFactor();
            double result = -factor;
            if (!double.IsFinite(result))
            {
                throw new ExpressionParseException("Calculation resulted in an overflow or non-finite value.");
            }
            return result;
        }

        // Parentheses
        if (Current.Type == TokenType.LeftParen)
        {
            Token openParen = Current;
            _position++;

            if (!IsAtEnd && Current.Type == TokenType.RightParen)
            {
                throw new ExpressionParseException($"Empty parentheses at position {openParen.Position}.", openParen.Position);
            }

            double subExpr = ParseExpression();

            if (IsAtEnd || Current.Type != TokenType.RightParen)
            {
                throw new ExpressionParseException($"Mismatched parentheses. Missing closing ')' for opening '(' at position {openParen.Position}.", openParen.Position);
            }

            _position++; // consume ')'
            return subExpr;
        }

        // Number literal
        if (Current.Type == TokenType.Number)
        {
            double value = Current.Value;
            _position++;
            return value;
        }

        // Unexpected token
        if (Current.Type == TokenType.RightParen)
        {
            throw new ExpressionParseException($"Unexpected closing parenthesis ')' at position {Current.Position}.", Current.Position);
        }

        throw new ExpressionParseException($"Unexpected operator '{Current.Text}' at position {Current.Position}. Expected an operand.", Current.Position);
    }

    private static bool IsOperator(TokenType type) =>
        type is TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide or TokenType.Modulus;
}
