bca-toolset
===========

BCADataAdviceToolset is a simple application to import BC Assessment Data Advice .dat files into a database.  

It was written by the Selkirk Geospatial Research Centre (SGRC) for the Regional District of Kootenay Boundary (RDKB).  The application processes BC Assessment dat files and imports to a database.  The fields being imported from the dat files are specific to RDKB data needs...all fields do not get imported.

The app is written in C# 3.5 and uses NHibernate to write to any database, although currently configured to send data to a MS Access database.  Logging is done with log4net.  The msi packaging is done with WixToolset 3.6.

There are two applications: wpf gui and command-line.  See bcatools app for command line usage.

This library is free software; you can redistribute it and/or modify it under
the terms of the GNU General Public License as published by the Free
Software Foundation; either version 3 of the License, or (at your option)
any later version.


To install:

Choose to download msi from latest downloads directory.  Ensure the application is always run as admin (otherwise log files don't get created...might be issue with my wix configuration)


A skeleton ms access db exists in downloads/empty-db to show schema of data imported
