using System;

public class View{

}

[Serializable]
public class ResponseLoginByPin{

    public string username;
    public string language;
    public string firstname;
    public string lastname;
    public string mail;
    public string cellphone;
    public int remainingVisits;
    public string maxPackageEndDate;
    public int credit;
    public roles[] roles;
    public packageContained[] packages;
    public takenClass lastClass;
}

[Serializable]
public class roles
{
    public string name;
}

[Serializable]
public class packageContained
{
    public int id;
    public string startDate;
    public string endDate;
    public long amount;
    public string language;
    public bool active;
    public package package;

}

[Serializable]
public class package
{
    public int id;
    public string name;
    public int duration;
    public int visits;
    public long price;
    public bool active;
    public string createDate;
}

[Serializable] 
public class takenClass
{
    public int id;
    public bool active;
    public string createDate;
    public Class Class;
}

[Serializable] 
public class Class
{
    public int id;
    public string classDate;
    public string startDate;
    public string endDate;
    public string description;
    public bool instructorCheckin;

}

[Serializable]
public class Register
{
    public string username;
    public string language = "es_MX";
    public string image = null;
    public string firstname;
    public string lastname = "";
    public string mail;
    public string cellphone;
    public string preferentialHour = "03:00";
    public string imageFile = "aquivaimagen";
}

[Serializable]
public class classes_read
{
    public Classes[] classes;
}

[Serializable]
public class Instructor
{
    public string firstname;
    public string lastname;
}

[Serializable]
public class Classes
{
    public int id;
    public string classDate;
    public string startDate;
    public string endDate;
    public string description;
    public string classroom;
    public int places;
    public string clients;
    public bool instructorCheckin;
    public bool active;
    public string createDate;
    public string updateDate;
    public Instructor instructor;
    public ClassSchedule @class_schedule;
    public preRegisters[] preregisters;
}

[Serializable]
public class preRegisters
{
    public string client;
}

[Serializable]
public class clientEmails
{
    public string client;
}

[Serializable]
public class ClassSchedule
{
    public string id;
    public string startHour;
    public string endHour;
    public string days;
    public string description;
    public string classroom;
    public string places;
    public bool active;
    public string createDate;
    public string updateDate;
    public string defaultInstructor;
}

[Serializable]
public class paquete
{
    public int id;
    public string name;
    public int duration;
    public int visits;
    public int price;
    public bool active;
    public string createDate;
    public string updateDate;
}

[Serializable]
public class grupoPaquetes
{
    public paquete[] packages;
}