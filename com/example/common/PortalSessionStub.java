package com.example.common;

import javax.servlet.http.HttpSession;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

// This simulates accessing session details like in C# PortalSession.
// In a real app, user details would come from Spring Security context or HttpSession.
public class PortalSessionStub {

    private static final String USER_ID_KEY = "portal_user_id";
    private static final String INSURER_USER_ID_KEY = "portal_insurer_user_id";
    private static final String ROLE_NAME_KEY = "portal_role_name";
     private static final String USER_LOGIN_ID_KEY = "portal_user_login_id"; // Added for file name

    private static HttpSession getSession() {
        ServletRequestAttributes attr = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        if (attr != null) {
            return attr.getRequest().getSession(false); // false means don't create if it doesn't exist
        }
        return null;
    }

    // Dummy getters - In a real app, populate these during login
    public static int getUserID() {
        HttpSession session = getSession();
        // Return dummy ID or value from session
        return (session != null && session.getAttribute(USER_ID_KEY) != null) ? (Integer) session.getAttribute(USER_ID_KEY) : 1; // Dummy default
    }

     public static String getUserLoginID() {
        HttpSession session = getSession();
        // Return dummy login ID or value from session
        return (session != null && session.getAttribute(USER_LOGIN_ID_KEY) != null) ? (String) session.getAttribute(USER_LOGIN_ID_KEY) : "dummyuser"; // Dummy default
    }


    public static int getInsurerUserID() {
         HttpSession session = getSession();
        // Return dummy ID or value from session
        return (session != null && session.getAttribute(INSURER_USER_ID_KEY) != null) ? (Integer) session.getAttribute(INSURER_USER_ID_KEY) : 101; // Dummy default
    }

     public static String getRoleName() {
        HttpSession session = getSession();
        // Return dummy role or value from session
        return (session != null && session.getAttribute(ROLE_NAME_KEY) != null) ? (String) session.getAttribute(ROLE_NAME_KEY) : "Admin"; // Dummy default
     }

    // Dummy method to set session attributes (e.g., after login)
    public static void setSessionAttributes(int userId, String userLoginId, int insurerUserId, String roleName) {
         ServletRequestAttributes attr = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
         if (attr != null) {
             HttpSession session = attr.getRequest().getSession(true); // true means create if doesn't exist
             session.setAttribute(USER_ID_KEY, userId);
             session.setAttribute(USER_LOGIN_ID_KEY, userLoginId);
             session.setAttribute(INSURER_USER_ID_KEY, insurerUserId);
             session.setAttribute(ROLE_NAME_KEY, roleName);
             System.out.println("PortalSessionStub: Set session attributes for user: " + userLoginId);
         } else {
             System.err.println("PortalSessionStub: Cannot get RequestAttributes outside of a web request context.");
         }
    }


    // Add other session properties/methods as needed
}