"use server";

import React from "react";
import Heading from "../components/heading/heading";
import { getAccessToken, getSession } from "../services/auth.service";
import AuthTest from "./auth-test";

const Session = async () => {
  const session = await getSession();
  const token = await getAccessToken();
  return (
    <div>
      <Heading title="Session dashboard" />
      <div className="bg-blue-200 rounded-lg shadow-md p-5">
        <h3 className="text-lg">Session data</h3>
        <pre>{JSON.stringify(session, null, 2)}</pre>
      </div>
      <div className="mt-4">
        <AuthTest token={token?.access_token} />
      </div>
      <div className="bg-green-200 shadow-md rounded-lg p-5 mt-4 overflow-auto">
        <h3 className="text-lg">Token data</h3>
        <pre>{JSON.stringify(token, null, 2)}</pre>
      </div>
    </div>
  );
};

export default Session;
