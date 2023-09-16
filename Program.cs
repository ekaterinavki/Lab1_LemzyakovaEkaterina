using System;
using System.ComponentModel.Design;
using System.IO;
using System.Text.RegularExpressions;


class Program
{
    static string LogFilePath = "log.txt";
    static void Main(string[] args)
    {
        //входные данные
        bool? success = null;

        string login = GetValidLogin();//Пример: +1-123-456-7890
        string password = GetValidPassword();//Пример: Password123!
        string confirmPassword = GetConfirmPassword(password);

        string result = success.HasValue ? success.Value.ToString() : "False";
        string message = success.HasValue ? "" : "Вывод: " + GetRegistrationErrorMessage(password, confirmPassword);

        Console.WriteLine("Строка1 (результат): " + result);
        Console.WriteLine("Строка2 (сообщение): " + message);

        Console.ReadKey();
    }

    //метод считывания логина с консоли и проверка его правильности
    static string GetValidLogin()
    {
        Console.WriteLine("Введите логин:");
        string login = Console.ReadLine();

        if (IsValidPhoneNumber(login) || IsValidEmail(login) || IsValidString(login))
        {
            if (IsValidString(login) && login.Length >= 5 && IsUniqueLogin(login))
            {
                return login;
            }
            else
            {
                Console.WriteLine("Логин должен содержать минимум 5 символов и состоять только из латиницы, цифр и знака подчеркивания.");
                return GetValidLogin();
            }
        }
        else
        {
            Console.WriteLine("Неправильный формат логина. Логин должен быть в формате телефона (+x-xxx-xxx-xxxx), электронной почты (xxx@xxx.xxx) или просто строки символов.");
            return GetValidLogin();
        }
    }

    //проверка формата логина
    static bool IsValidPhoneNumber(string phoneNumber)
    {
        string pattern = @"^\+\d{1,}-\d{3}-\d{3}-\d{4}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }

    static bool IsValidEmail(string email)
    {
        string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        return Regex.IsMatch(email, pattern);
    }

    static bool IsValidString(string str)
    {
        string pattern = @"^[a-zA-Z0-9_]+$";
        return Regex.IsMatch(str, pattern);
    }

    static bool IsUniqueLogin(string login)
    {
        // Проверка на уникальность логина
        string[] predefinedLogins = { "admin", "user1", "user2" };
        foreach(string predefinedLogin in predefinedLogins)
        {
            if(predefinedLogin.ToLower() == login.ToLower())
            {
                return false;
            }
        }

        return true;
    }

    //метод считывания пароля с консоли и проверка его правильности
    static string GetValidPassword()
    {
        Console.WriteLine("Введите пароль:");
        string password = Console.ReadLine();

        if (IsValidPassword(password))
        {
            return password;
        }
        else
        {
            Console.WriteLine("Пароль должен содержать минимум 7 символов, латиницу, цифры и спецсимволы, а также минимум одну букву в верхнем и нижнем регистре.");
            return GetValidPassword();
        }
    }

    //проверка формата пароля
    static bool IsValidPassword(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{7,}$"; ;
        return Regex.IsMatch(password, pattern);
    }

    //метод который считывает подтверждение пароля с консоли и проверяет его совпадение с введенным паролем
    static string GetConfirmPassword(string password)
    {
        Console.WriteLine("Подтвердите пароль:");
        string confirmPassword = Console.ReadLine();

        if (confirmPassword == password)
        {
            return confirmPassword;
        }
        else
        {
            Console.WriteLine("Подтверждение пароля не совпадает с введенным паролем. Пожалуйста, попробуйте снова.");
            return GetConfirmPassword(password);
        }
    }

    //метод который возвращает сообщение об ошибке при регистрации
    static string GetRegistrationErrorMessage(string password, string confirmPassword)
    {
        string errorMessage = "";

        if (password.Length < 8)
            errorMessage = "Пароль должен быть не менее 8 символов";
        else if (password != confirmPassword)
            errorMessage = "Подтверждение пароля не совпадает";
        else if (password == confirmPassword)
            errorMessage = "Регистрация прошла успешно";
        else
            errorMessage = "Неизвестная ошибка";
        return errorMessage;
    }

    //Методы записывающие информацию о регистрации в файл и выводят ее на консоль
    static void LogSuccessfulRegistration(string login, string password, string confirmPassword)
    {
        string logMessage = string.Format("{0} - {1} - {2} - {3} - Успешная регистрация",
        DateTime.Now.ToString(), login, MaskPassword(password), MaskPassword(confirmPassword));

        LogToFile(logMessage);
        Console.WriteLine(logMessage);
    }

    static void LogUnsuccessfulRegistration(string login, string password, string confirmPassword, string errorMessage)
    {
        string logMessage = string.Format("{0} - {1} - {2} - {3} - {4}",
        DateTime.Now.ToString(), login, MaskPassword(password), MaskPassword(confirmPassword), errorMessage);

        LogToFile(logMessage);
        Console.WriteLine(logMessage);
    }

    static string MaskPassword(string password)
    {
        // Маскирование пароля
        return new string('*', password.Length);
    }

    //записывает лог в файл "log.txt"
    static void LogToFile(string message)
    {
        using (StreamWriter sw = File.AppendText(LogFilePath))
        {
            sw.WriteLine(message);
        }
    }

    //выводит лог в консоль
    static void LogToConsole(string message)
    {
        Console.WriteLine(message);
    }
}