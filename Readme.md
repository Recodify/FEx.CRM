# Recodify FEx.CRM

FEx.CRM is a Dynamics 365 (CRM) solution that allows the scheduled synchronisation of Foreign Exchange rates between a number of rates services and your CRM instance.

Easy to install and configure with a flexible scheudling system and active monitoring and support included  with your subscription. 

## Installation

FEx.CRM is installed just like any other Dynamics 365 solution. Simply download the solution package and import it to your Dynamics 365 instance. For a detailed guide on installing a CRM solution see: [Dynamics Solution Installation Guide](https://bitbucket.org/recodify/fex.crm/wiki/Dynamics%20Solution%20Installation%20Guide)

## Configuration

Configuration is a simple process that requires you to specify just 5 fields. The configuration should be self explantory and can be access from the main menu. For a detailed guide to the configuration see: [FEx.CRM Configuration Guide](https://bitbucket.org/recodify/fex.crm/wiki/Configuration)

![FEx Configuration](http://recodify.co.uk/hostedimages/configarea.png)

## Rate Synchronisation

The rate synchronisation will take place at on a date and time as specified by your configuration. When the synchronisation is run, it will interrogate your chosen Data Source source and try to match the rate data returned to the currencies that you have created in your CRM. To check which currencies you have setup or to create new ones: Settings > Business Management > Currencies

![CurrencyConfiguration](http://recodify.co.uk/hostedimages/currencies2.png)

## Monitoring

The date and status of the the last synchronisation performed can be seen in the details section of your Foreign Exchange configuration:

![Last Run Status](http://recodify.co.uk/hostedimages/lastrunstatus.png)

Recodify FEx.CRM includes active (proactive) monitoring by Recodify and so in the event of an error, we will probably already be investigating and resolving the issue. When an error is encountered, the synchronisation will be rescheduled again according to a progressive backoff and retry algorithm: Attempt No * 10 mins. E.g. 1st error = run again in 10 mins, 3rd error = run again in 30 mins. The maximum retry interval is 61 mins. So hopefully during one of these runs, the problem will have been resolved and a successful synchronisation will be performed. If you still have problems there are two options.

##### 1. Raise an Issue

[Issue Tracker](https://bitbucket.org/recodify/fex.crm/issues).

##### 2. Read the Monitoring Guide

[FEx.CRM Monitoring Guide](https://bitbucket.org/recodify/fex.crm/wiki/Monitoring)

## Support

This wiki is the primary source of documentation and help but if there is something missing please get in touch with us at <info@recodify.co.uk>. Alternatively, if you have encountered a bug or have a feature request, please raise a issue using the [Issue Tracker](https://bitbucket.org/recodify/fex.crm/issues).