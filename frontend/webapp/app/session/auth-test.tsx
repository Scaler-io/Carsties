"use client";

import React, { useState } from "react";
import { updateAuction } from "../services/auction.service";
import { Button } from "flowbite-react";

const AuthTest = () => {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState();

  function doUpdate() {
    setResult(undefined);
    setLoading(true);
    updateAuction()
      .then((res: any) => setResult(res))
      .finally(() => setLoading(false));
  }

  return (
    <div className="flex items-center gap-4">
      <Button outline isProcessing={loading} onClick={doUpdate}>
        Test auth
      </Button>

      <div>{JSON.stringify(result, null, 2)}</div>
    </div>
  );
};

export default AuthTest;
