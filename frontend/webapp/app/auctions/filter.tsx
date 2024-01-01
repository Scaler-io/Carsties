import { Button, ButtonGroup } from "flowbite-react";
import React from "react";

interface Props{
  pageSize: number;
  setPageSize: (size: number) => void;
}

const pageSizeOptions = [4, 8, 12];

const Filter = ({pageSize, setPageSize}: Props) => {
  return <div className="flex justify-between items-center mb-4">
    <div>
      <span className="uppercase text-sm text-gray-500 mr-2">Page size</span>
      <ButtonGroup>
        {pageSizeOptions.map((value, i) => (
          <Button 
            key={i} 
            onClick={() => {setPageSize(value)}}
            color={`${pageSize === value ? 'red' : 'gray'}`}
            className="focus:ring-0 focus:text-lightRed"
          >
            {value}
          </Button>
        ))}
      </ButtonGroup>
    </div>
  </div>;
};

export default Filter;
