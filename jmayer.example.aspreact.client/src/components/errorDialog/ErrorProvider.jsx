import React, { createContext, useContext, useState } from 'react';

//Create the context for the global error states.
const ErrorContext = createContext();

//Used to manage the global states for the error dialog.
//@param {object} props The properties accepted by the component.
//@param {array} props.children The children objects 
export const ErrorProvider = ({ children }) => {
    const [errorVisible, setErrorVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    //Hides the error dialog.
    const hideError = () => {
        setErrorMessage('');
        setErrorVisible(false);
    };

    //Opens the error dialog.
    //@param {string} error The error to display to the user.
    const showError = (message) => {
        setErrorMessage(message);
        setErrorVisible(true);
    };

    return (
        <ErrorContext.Provider value={{ errorVisible, errorMessage, showError, hideError }}>
            {children}
        </ErrorContext.Provider>
    );
};

//A hook used by the components to interact with the error context.
export const useError = () => {
    return useContext(ErrorContext);
}