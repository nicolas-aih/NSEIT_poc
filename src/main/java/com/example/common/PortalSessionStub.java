// File: src/main/java/com/yourcompany/common/PortalSessionStub.java
package com.example.common;

import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import jakarta.servlet.http.HttpSession;

// This simulates accessing session details like in C# PortalSession.
// In a real app, user details would come from Spring Security context or HttpSession.
// Requires spring-boot-starter-web which sets up RequestContextListener
public class PortalSessionStub {

    private static final String USER_ID_KEY = "portal_user_id";
    private static final String INSURER_USER_ID_KEY = "portal_insurer_user_id";
    private static final String ROLE_NAME_KEY = "portal_role_name";
     private static final String USER_LOGIN_ID_KEY = "portal_user_login_id";

    private static HttpSession getSession() {
        ServletRequestAttributes attr = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        if (attr != null) {
            // Use true to create session if it doesn't exist, or false if you only want existing sessions
            // For basic stubbing, creating might be simpler
            return attr.getRequest().getSession(true);
        }
         // Should not happen in a web request context if spring-boot-starter-web is used correctly
        System.err.println("PortalSessionStub: Cannot get RequestAttributes outside of a web request context.");
        return null;
    }

    // Dummy getters - In a real app, populate these during login and retrieve from session
    public static int getUserID() {
        HttpSession session = getSession();
        // Return dummy ID or value from session. Dummy value 1 for testing if session is null.
        return (session != null && session.getAttribute(USER_ID_KEY) != null) ? (Integer) session.getAttribute(USER_ID_KEY) : 1;
    }

     public static String getUserLoginID() {
        HttpSession session = getSession();
        // Return dummy login ID or value from session. Dummy value "dummyuser" for testing.
        return (session != null && session.getAttribute(USER_LOGIN_ID_KEY) != null) ? (String) session.getAttribute(USER_LOGIN_ID_KEY) : "dummyuser";
    }


    public static int getInsurerUserID() {
         HttpSession session = getSession();
        // Return dummy ID or value from session. Dummy value 101 for testing.
        return (session != null && session.getAttribute(INSURER_USER_ID_KEY) != null) ? (Integer) session.getAttribute(INSURER_USER_ID_KEY) : 101;
    }

     public static String getRoleName() {
        HttpSession session = getSession();
        // Return dummy role or value from session. Dummy value "Admin" for testing.
        return (session != null && session.getAttribute(ROLE_NAME_KEY) != null) ? (String) session.getAttribute(ROLE_NAME_KEY) : "Admin";
     }

     // Dummy setter for testing login simulation
     public static void simulateLogin(int userId, String userLoginId, int insurerUserId, String roleName) {
          HttpSession session = getSession();
          if (session != null) {
              session.setAttribute(USER_ID_KEY, userId);
              session.setAttribute(USER_LOGIN_ID_KEY, userLoginId);
              session.setAttribute(INSURER_USER_ID_KEY, insurerUserId);
              session.setAttribute(ROLE_NAME_KEY, roleName);
              System.out.println("PortalSessionStub: Simulated login, session attributes set.");
          } else {
              System.err.println("PortalSessionStub: Could not simulate login outside of a request context.");
          }
     }


    // Add other session properties/methods as needed
}