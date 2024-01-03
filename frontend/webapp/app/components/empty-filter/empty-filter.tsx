"use client";

import { useParamsStore } from "@/hooks/useParamsStore";
import React from "react";
import Heading from "../heading/heading";
import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";

interface Props {
  title?: string;
  subtitle?: string;
  showReset?: boolean;
  showLogin?: boolean;
  callbackUrl?: string;
}

const EmptyFilter = ({
  title = "No matches for this filter",
  subtitle = "Try chanhing or restting the filer",
  showReset,
  showLogin,
  callbackUrl,
}: Props) => {
  const reset = useParamsStore((state) => state.reset);

  return (
    <div className="h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg">
      <Heading title={title} subtitle={subtitle} center />
      <div className="mt-4">
        {showReset && (
          <Button outline onClick={reset} color="info">
            Remove filters
          </Button>
        )}
        {showLogin && (
          <Button
            outline
            onClick={() => signIn("id-server", { callbackUrl })}
            color="info">
            Login
          </Button>
        )}
      </div>
    </div>
  );
};

export default EmptyFilter;
