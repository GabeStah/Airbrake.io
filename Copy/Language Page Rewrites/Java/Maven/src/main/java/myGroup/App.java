package myGroup;

import org.apache.log4j.LogManager;
import org.apache.log4j.Logger;

/**
 * Hello world!
 *
 */
public class App 
{
    private static final Logger logger = LogManager.getLogger(App.class);
    public static void main( String[] args )
    {
        try {
            int num[] = {1, 2, 3, 4};
            System.out.println(num[5]);
        }
        catch (Exception e) {
            logger.error(e);
        }
    }
}