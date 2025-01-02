import React, { createContext, useContext, useState } from 'react';

//Create the context for the global error states.
const ErrorContext = createContext();

//The function returns the error provider for the error context.
//@param {object} props The properties accepted by the component.
//@param {array} props.children The children components.
export const ErrorProvider = ({ children }) => {
    const [errorVisible, setErrorVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    //The function hides the error dialog.
    const hideError = () => {
        setErrorMessage('');
        setErrorVisible(false);
    };

    //The function opens the error dialog.
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

//The function returns a hook used by the components to interact with the error context.
export const useError = () => {
    return useContext(ErrorContext);
}