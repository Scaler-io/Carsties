"use client";

import React from "react";
import { signIn } from "next-auth/react";
import { Button } from "flowbite-react";

const LoginButton = () => {
  return (
    <Button onClick={() => signIn("id-server", { callbackUrl: "/" })}>
      Login
    </Button>
  );
};

export default LoginButton;
